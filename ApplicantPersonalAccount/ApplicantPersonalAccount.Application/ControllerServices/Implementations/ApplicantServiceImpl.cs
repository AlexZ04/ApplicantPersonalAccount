using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models;
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

        public ApplicantServiceImpl(
            DirectoryDataContext directoryContext,
            IApplicationRepository applicationRepository)
        {
            _directoryContext = directoryContext;
            _applicationRepository = applicationRepository;
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
                SigningTime = DateTime.UtcNow.ToUniversalTime()
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
    }
}
