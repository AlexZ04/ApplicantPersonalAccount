using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using ApplicantPersonalAccount.Persistence.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class ApplicantServiceImpl : IApplicantService
    {
        private readonly IMessageProducer _messageProducer;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IApplicationRepository _applicationRepository;

        public ApplicantServiceImpl(
            IApplicationRepository applicationRepository,
            IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            _applicationRepository = applicationRepository;
        }

        public async Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = userId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_INFO_FOR_EVENTS);
            ResponseProcessor.ProcessResponse(result);

            var infoEventsData = JsonSerializer.Deserialize<InfoForEventsEntity>(result, _jsonOptions)!;

            result = await rpcClient.CallAsync(userId.ToString(), RabbitQueues.GET_USER_BY_ID);
            ResponseProcessor.ProcessResponse(result);

            var userData = JsonSerializer.Deserialize<UserEntity>(result, _jsonOptions)!;

            var userInfo = new ApplicantInfoForEventsModel
            {
                EducationPlace = infoEventsData.EducationPlace,
                SocialNetworks = infoEventsData.SocialNetwork,
                Address = userData.Address
            };

            return userInfo;
        }

        public void EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId)
        {
            var request = new BrokerEditInfoForEventsDTO
            {
                UserId = userId,
                Model = editedInfo,
            };

            _messageProducer.SendMessage(request, RabbitQueues.EDIT_INFO_FOR_EVENTS);
        }

        public async Task SignToNotifications(string userEmail)
        {
            var rpcClient = new RpcClient();
            var request = new SubscriptionToNotificationDTO 
            {
                Subscribe = true,
                UserEmail = userEmail
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.SUBS);

            if (result != BrokerMessages.USER_IS_SUCCESSFULY_SIGNED)
                throw new InvalidActionException(ErrorMessages.USER_IS_SIGNED);
        }

        public async Task UnsignFromNotifications(string userEmail)
        {
            var rpcClient = new RpcClient();
            var request = new SubscriptionToNotificationDTO
            {
                Subscribe = false,
                UserEmail = userEmail
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.SUBS);

            if (result != BrokerMessages.USER_IS_SUCCESSFULY_UNSIGNED)
                throw new InvalidActionException(ErrorMessages.USER_IS_UNSIGNED);
        }

        public async Task<bool> CanUserEdit(Guid userId)
        {
            var status = await GetUserEnteranceStatus(userId);

            if (status == EnteranceStatus.Closed)
                return true;
            return false;
        }

        private async Task<EnteranceStatus> GetUserEnteranceStatus(Guid userId)
        {
            var enterance = await _applicationRepository.GetUserEnterance(userId);

            return enterance.Status;
        }
    }
}
