using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Repositories;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.Document.MessageListeners
{
    public class GetUserDocumentsListener : BaseMessageListener<GuidRequestDTO>
    {
        public GetUserDocumentsListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_USER_DOCUMENTS) { }

        protected override async Task<string?> ProcessMessage(
            GuidRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var documentRepository = serviceProvider.GetRequiredService<IDocumentRepository>();

            var documents = await documentRepository.GetUserDocuments(
                FileDocumentType.Educational,
                message.Id);

            return JsonSerializer.Serialize(documents);
        }
    }
}
