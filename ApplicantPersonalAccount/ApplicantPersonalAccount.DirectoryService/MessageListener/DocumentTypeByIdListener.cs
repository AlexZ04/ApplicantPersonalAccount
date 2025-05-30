using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.DirectoryService.Services;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.DirectoryService.MessageListener
{
    public class DocumentTypeByIdListener : BaseMessageListener<GuidRequestDTO>
    {
        public DocumentTypeByIdListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_DOCUMENT_TYPE_BY_ID) { }

        protected override async Task<string?> ProcessMessage(
            GuidRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var directoryInfoService = serviceProvider.GetRequiredService<IDirectoryInfoService>();

            try
            {
                var data = await directoryInfoService.GetDocumentTypeById(message.Id);
                return JsonSerializer.Serialize(data);
            }
            catch
            {
                return "";
            }

        }
    }
}
