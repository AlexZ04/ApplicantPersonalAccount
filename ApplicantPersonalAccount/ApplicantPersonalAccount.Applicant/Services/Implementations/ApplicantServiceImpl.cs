using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Persistence.Repositories;
using System.Text.Json;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class ApplicantServiceImpl : IApplicantService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMessageProducer _messageProducer;

        public ApplicantServiceImpl(
            IApplicationRepository applicationRepository,
            IMessageProducer messageProducer)
        {
            _applicationRepository = applicationRepository;
            _messageProducer = messageProducer;
        }

        public async Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId)
        {
            var rpcClient = new RpcClient();

            string result = await rpcClient.CallAsync(userId.ToString(), RabbitQueues.GET_INFO_FOR_EVENTS);
            ProcessResponse(result);

            var data = JsonSerializer.Deserialize<ApplicantInfoForEventsModel>(result)!;

            return data;
        }

        public Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId)
        {
            throw new NotImplementedException();
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

        private void ProcessResponse(string response)
        {
            if (response == null)
                throw new ProcessingException();
        }
    }
}
