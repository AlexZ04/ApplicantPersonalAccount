using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using System.Text.Json;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class ApplicationServiceImpl : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationServiceImpl(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task AddProgram(EducationProgramApplicationModel program, Guid userId)
        {
            var enterance = await _applicationRepository.GetUserEnterance(userId);

            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = userId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_EDUCATION_PROGRAM_BY_ID);
            if (result == null)
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            var educationProgram = JsonSerializer.Deserialize<EducationProgram>(result)!;

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
                CreateTime = DateTime.UtcNow.ToUniversalTime()
            };

            await _applicationRepository.AddProgramToEnterance(newProgram, enterance);
        }

        public async Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProgram(Guid programId, Guid userId)
        {
            await _applicationRepository.DeleteProgram(programId, userId);
        }

        private async Task CheckEducationLevel(EnteranceEntity enterance, string selectedEducationLevelName)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = enterance.Programs[0].ProgramId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_EDUCATION_PROGRAM_BY_ID);
            if (result == null)
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            var selectedProgram = JsonSerializer.Deserialize<EducationProgram>(result)!;

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
