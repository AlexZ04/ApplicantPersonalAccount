using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.DirectoryService.Services;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.DirectoryService.MessageListener
{
    public class DocumentTypeListener : BaseMessageListener<BrokerRequestDTO>
    {
        public DocumentTypeListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_DOCUMENT_TYPE) { }

        protected override async Task<string?> ProcessMessage(
            BrokerRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var directoryInfoService = serviceProvider.GetRequiredService<IDirectoryInfoService>();

            var data = await directoryInfoService.GetDocumentTypes();

            return JsonSerializer.Serialize(data);
        }
    }
}
