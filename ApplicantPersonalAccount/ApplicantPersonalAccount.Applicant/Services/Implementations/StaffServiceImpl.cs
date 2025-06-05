
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class StaffServiceImpl : IStaffService
    {
        private readonly ApplicationDataContext _applicationContext;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILogger<StaffServiceImpl> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public StaffServiceImpl(
            ApplicationDataContext applicationContext, 
            IApplicationRepository applicationRepository,
            ILogger<StaffServiceImpl> logger)
        {
            _applicationContext = applicationContext;
            _applicationRepository = applicationRepository;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        public async Task AttachEnteranceToManager(Guid userId, Guid managerId)
        {
            var enterance = await _applicationRepository.GetUserEnterance(userId);

            var manager = await GetUserById(managerId);

            if (manager.Role == Role.Applicant)
                throw new InvalidActionException(ErrorMessages.USER_IS_NOT_MANAGER);

            enterance.ManagerId = managerId;

            await _applicationContext.SaveChangesAsync();

            _logger.LogInformation($"User {userId} enterance is now attached to {manager.Email}");
        }

        public async Task UnattachEnteranceFromManager(Guid userId)
        {
            var enterance = await _applicationRepository.GetUserEnterance(userId);

            if (enterance.ManagerId != null)
                throw new InvalidActionException(ErrorMessages.ENTERANCE_IS_NOT_ATTACHED_TO_ANYONE);

            enterance.ManagerId = null;
            await _applicationContext.SaveChangesAsync();

            _logger.LogInformation($"User {userId} enterance is now free");
        }

        private async Task<UserEntity> GetUserById(Guid userId)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = userId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_USER_BY_ID);
            if (result == null)
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

            var userData = JsonSerializer.Deserialize<UserEntity>(result, _jsonOptions)!;

            return userData;
        }
    }
}
