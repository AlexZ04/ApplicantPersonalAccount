using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Persistence.Repositories;

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

        public Task AddProgram(EducationProgramApplicationModel program, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProgram(Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentType>> GetDocumentTypes()
        {
            throw new NotImplementedException();
        }

        public Task<ProgramPagedList> GetListOfPrograms(
            string faculty,
            string educationForm,
            string language,
            string code,
            string name,
            int page = 1,
            int size = 5)
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
        }
    }
}
