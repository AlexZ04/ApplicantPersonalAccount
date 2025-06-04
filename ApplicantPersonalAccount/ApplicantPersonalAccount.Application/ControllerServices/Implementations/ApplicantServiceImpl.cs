using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Application.ControllerServices.Implementations
{
    public class ApplicantServiceImpl : IApplicantService
    {
        private readonly DirectoryDataContext _directoryContext;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDataContext _applicationContext;
        private readonly IDocumentRepository _documentRepository;

        public ApplicantServiceImpl(
            DirectoryDataContext directoryContext,
            IApplicationRepository applicationRepository,
            IUserRepository userRepository,
            ApplicationDataContext applicationContext,
            IDocumentRepository documentRepository)
        {
            _directoryContext = directoryContext;
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
            _applicationContext = applicationContext;
            _documentRepository = documentRepository;
        }

        public async Task<ProgramPagedList> GetListOfPrograms(
            string faculty, 
            string educationForm, 
            string language, 
            string code, 
            string name, 
            int page = 1, 
            int size = 5)
        {
            if (_directoryContext.EducationPrograms.FirstOrDefault(e => e.Name.Contains("")) == null)
                throw new EarlyActionException(ErrorMessages.DICTIONARY_IS_NOT_LOADED);

            name = name ?? string.Empty;
            var programs = _directoryContext.EducationPrograms
                .Include(p => p.EducationLevel)
                .Include(p => p.Faculty)
                .AsNoTracking()
                .Where(p => p.Name.Contains(name));

            if (faculty != null)
                programs = programs.Where(p => p.Faculty.Name.Contains(faculty));

            if (language != null)
                programs = programs.Where(p => p.Language.Contains(language));

            if (code != null)
                programs = programs.Where(p => p.Code.Contains(code));

            if (educationForm != null)
                programs = programs.Where(p => p.EducationForm.Contains(educationForm));

            var totalCount = await programs.CountAsync();

            programs = programs
                .Skip((page - 1) * size)
                .Take(size);

            var pagination = new PageInfoModel
            {
                Current = page,
                Size = size,
                Count = (totalCount / size) + (totalCount % size > 0 ? 1 : 0)
            };

            var result = new ProgramPagedList
            {
                Programs = await programs.ToListAsync(),
                Pagination = pagination
            };

            return result;
        }

        public async Task SignToNotifications(Guid userId)
        {
            var isSigned = await _applicationRepository.IsUserSigned(userId);

            if (isSigned)
                throw new InvalidActionException(ErrorMessages.USER_IS_SIGNED);

            var signingInfo = new SignedToNotificationsEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SigningTime = DateTime.Now.ToUniversalTime()
            };

            await _applicationRepository.SignUser(signingInfo);
        }

        public async Task UnsignFromNotifications(Guid userId)
        {
            var isSigned = await _applicationRepository.IsUserSigned(userId);

            if (!isSigned)
                throw new InvalidActionException(ErrorMessages.USER_IS_UNSIGNED);

            await _applicationRepository.UnsignUser(userId);
        }

        public async Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId)
        {
            await _userRepository.EditInfoForEvents(editedInfo, userId);
        }

        public async Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId)
        {
            var infoForEvents = await _userRepository.GetInfoForEvents(userId);
            var user = await _userRepository.GetUserById(userId);

            var userInfo = new ApplicantInfoForEventsModel
            {
                EducationPlace = infoForEvents.EducationPlace,
                SocialNetworks = infoForEvents.SocialNetwork,
                Address = user.Address
            };

            return userInfo;
        }

        public async Task AddProgram(EducationProgramApplicationModel program, Guid userId)
        {
            var enterance = await _applicationRepository.GetUserEnterance(userId);

            var educationProgram = await _directoryContext.EducationPrograms
                .Include(p => p.Faculty)
                .Include(p => p.EducationLevel)
                .FirstOrDefaultAsync(p => p.Id == program.ProgramId);

            if (educationProgram == null)
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            if (enterance.Programs.Count > GeneralSettings.MAX_CHOSEN_PROGRAMS)
                throw new InvalidActionException(ErrorMessages.HAVE_MAX_PROGRAMS);

            var selectedEducationLevelName = educationProgram.EducationLevel.Name;

            if (enterance.Programs.Count() > 0)
                await CheckEducationLevel(enterance, selectedEducationLevelName);

            var userDocuments = await _documentRepository
                .GetUserDocuments(FileDocumentType.Educational, userId);

            var newProgram = new EnteranceProgramEntity
            {
                Id = Guid.NewGuid(),
                ProgramId = program.ProgramId,
                Priority = program.Priority,
                Enterance = enterance,
                CreateTime = DateTime.Now.ToUniversalTime()
            };

            await _applicationRepository.AddProgramToEnterance(newProgram, enterance);
        }

        public async Task DeleteProgram(Guid programId, Guid userId)
        {
            await _applicationRepository.DeleteProgram(programId, userId);
        }

        public async Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId)
        {
            // todo
        }

        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            var documentTypes = await _directoryContext.DocumentTypes
                .Include(t => t.EducationLevel)
                .Include(t => t.NextEducationLevels)
                .ToListAsync();

            return documentTypes;
        }

        private async Task CheckEducationLevel(EnteranceEntity enterance, string selectedEducationLevelName)
        {
            var selectedProgram = await _directoryContext.EducationPrograms
                    .Include(p => p.Faculty)
                    .Include(p => p.EducationLevel)
                    .FirstOrDefaultAsync(l => l.Id == enterance.Programs[0].ProgramId);

            if (selectedProgram == null)
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            var educationLevelName = selectedProgram.EducationLevel.Name;

            if (educationLevelName != selectedEducationLevelName
                && ((educationLevelName == "Специалитет" && selectedEducationLevelName == "Бакалавриат") ||
                (educationLevelName == "Бакалавриат" && selectedEducationLevelName == "Специалитет")))
                throw new InvalidActionException(ErrorMessages.CANT_HAVE_THIS_EDUCATION_LEVEL);
        }
    }
}
