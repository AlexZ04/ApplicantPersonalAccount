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
            rpcClient.Dispose();
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

            await CheckDocumentsCompatibility(userDocuments, selectedEducationLevelId);

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
                var currentDocument = await GetDocumentTypeById(document.Id, rpcClient);

                availableLevels.Add(currentDocument.EducationLevel.Id);

                foreach (var educationLevel in currentDocument.NextEducationLevels) 
                    availableLevels.Add(educationLevel.Id);
            }

            rpcClient.Dispose();

            if (!availableLevels.Contains(selectedEducationLevelId) && documents.Count > 0)
                throw new InvalidActionException(ErrorMessages.CANT_HAVE_THIS_EDUCATION_LEVEL);
        }

        private async Task CheckEditable(Guid userId)
        {
            var canEdit = await _applicantService.CanUserEdit(userId);

            if (!canEdit)
                throw new InvalidActionException(ErrorMessages.USER_CANT_EDIT_THIS_DATA);
        }

        private async Task<DocumentType> GetDocumentTypeById(Guid id, RpcClient rpcClient)
        {
            var request = new GuidRequestDTO
            {
                Id = id
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_DOCUMENT_TYPE_BY_ID);
            if (result == null || result == "null")
                throw new NotFoundException(ErrorMessages.DOCUMENT_TYPE_NOT_FOUND);

            var documentType = JsonSerializer.Deserialize<DocumentType>(result)!;

            return documentType;
        }
    }
}
