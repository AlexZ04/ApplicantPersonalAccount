using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class ApplicationServiceImpl : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicantService _applicantService;
        private readonly ApplicationDataContext _applicationDataContext;

        public ApplicationServiceImpl(
            IApplicationRepository applicationRepository,
            IApplicantService applicationService,
            ApplicationDataContext applicationContext)
        {
            _applicationRepository = applicationRepository;
            _applicantService = applicationService;
            _applicationDataContext = applicationContext;
        }

        public async Task AddProgram(EducationProgramApplicationModel program, Guid userId, string userRole)
        {
            if (userRole == "Applicant")
                await CheckEditable(userId);

            var enterance = await _applicationRepository.GetUserEnterance(userId, true);

            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = program.ProgramId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_EDUCATION_PROGRAM_BY_ID);
            if (result == null || result == "null")
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            var educationProgram = JsonSerializer.Deserialize<EducationProgram>(result)!;

            if (enterance.Programs.Count > GeneralSettings.MAX_CHOSEN_PROGRAMS)
                throw new InvalidActionException(ErrorMessages.HAVE_MAX_PROGRAMS);

            var selectedEducationLevelId = educationProgram.EducationLevel.Id;

            if (enterance.Programs.Count() > 0)
                await CheckEducationLevel(enterance, selectedEducationLevelId);

            request = new GuidRequestDTO
            {
                Id = userId
            };
            result = await rpcClient.CallAsync(request, RabbitQueues.GET_USER_DOCUMENTS);

            var userDocuments = JsonSerializer.Deserialize<List<DocumentEntity>>(result)!;
            rpcClient.Dispose();

            await CheckDocumentsCompatibility(userDocuments, selectedEducationLevelId);

            var usedProgramData = await _applicationDataContext.EnterancePrograms
                .Include(p => p.Enterance)
                .Where(p => p.Enterance.ApplicantId == userId && 
                    (p.Priority == program.Priority || p.ProgramId == program.ProgramId))
                .CountAsync();

            if (usedProgramData > 0)
                throw new InvalidActionException(ErrorMessages.PROGRAM_IS_USED);

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

        public async Task EditProgram(EducationProgramApplicationEditModel program, Guid programId,
            Guid userId, string userRole)
        {
            if (userRole == "Applicant")
                await CheckEditable(userId);

            var foundProgram = await _applicationDataContext.EnterancePrograms
                .FirstOrDefaultAsync(p => p.Id == programId);

            if (foundProgram == null)
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            var usedProgramData = await _applicationDataContext.EnterancePrograms
                .Include(p => p.Enterance)
                .Where(p => p.Enterance.ApplicantId == userId && p.Priority == program.Priority)
                .CountAsync();

            if (usedProgramData > 0)
                throw new InvalidActionException(ErrorMessages.PROGRAM_IS_USED);

            foundProgram.Priority = program.Priority;

            await _applicationDataContext.SaveChangesAsync();
        }

        public async Task DeleteProgram(Guid programId, Guid userId, string userRole)
        {
            if (userRole == "Applicant")
                await CheckEditable(userId);

            await _applicationRepository.DeleteProgram(programId, userId);
        }

        private async Task CheckEducationLevel(EnteranceEntity enterance, int selectedEducationLevelId)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = enterance.Programs[0].ProgramId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_EDUCATION_PROGRAM_BY_ID);
            rpcClient.Dispose();
            if (result == null || result == "null")
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            var selectedProgram = JsonSerializer.Deserialize<EducationProgram>(result)!;

            var educationLevelId = selectedProgram.EducationLevel.Id;

            if (educationLevelId != selectedEducationLevelId)
                throw new InvalidActionException(ErrorMessages.CANT_HAVE_THIS_EDUCATION_LEVEL);
        }

        private async Task CheckDocumentsCompatibility(List<DocumentEntity> documents, int selectedEducationLevelId)
        {
            var availableLevels = new HashSet<int>();
            var rpcClient = new RpcClient();

            foreach (var document in documents)
            {
                var currentEducationInfo = await GetEducatonInfo(document.Id, rpcClient);

                if (currentEducationInfo.DocumentTypeId != null)
                {
                    var currentDocument = await GetDocumentTypeById((Guid)currentEducationInfo.DocumentTypeId,
                        rpcClient);

                    availableLevels.Add(currentDocument.EducationLevel.Id);

                    foreach (var educationLevel in currentDocument.NextEducationLevels)
                        availableLevels.Add(educationLevel.Id);
                }
            }

            rpcClient.Dispose();

            if (!availableLevels.Contains(selectedEducationLevelId) && documents.Count > 0
                && availableLevels.Count > 0)
                throw new InvalidActionException(ErrorMessages.CANT_HAVE_THIS_EDUCATION_LEVEL);
        }

        private async Task CheckEditable(Guid userId)
        {
            var canEdit = await _applicantService.CanUserEdit(userId);

            if (!canEdit)
                throw new InvalidActionException(ErrorMessages.USER_CANT_EDIT_THIS_DATA);
        }

        private async Task<EducationInfoEntity> GetEducatonInfo(Guid id, RpcClient rpcClient)
        {
            var request = new GuidRequestDTO
            {
                Id = id
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_EDUCATION_INFO);
            if (result == null || result == "null" || result == "")
                throw new NotFoundException(ErrorMessages.EDUCATION_INFO_NOT_FOUND);

            var educationInfo = JsonSerializer.Deserialize<EducationInfoEntity>(result)!;

            return educationInfo;
        }

        private async Task<DocumentType> GetDocumentTypeById(Guid id, RpcClient rpcClient)
        {
            var request = new GuidRequestDTO
            {
                Id = id
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_DOCUMENT_TYPE_BY_ID);
            if (result == null || result == "null" || result == "")
                throw new NotFoundException(ErrorMessages.DOCUMENT_TYPE_NOT_FOUND);

            var documentType = JsonSerializer.Deserialize<DocumentType>(result)!;

            return documentType;
        }
    }
}
