using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.DirectoryService.Services;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using RabbitMQ.Client.Events;

namespace ApplicantPersonalAccount.DirectoryService.MessageListener
{
    public class RequestImportListener : BaseMessageListener<ImportRequestDTO>
    {
        public RequestImportListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.IMPORT_REQUEST) { }

        protected override async Task<string?> ProcessMessage(
            ImportRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var directoryLoadingService = serviceProvider.GetRequiredService<IDirectoryLoadingService>();

            try
            {
                await directoryLoadingService.RequestDictImport(message.Type);
            }
            catch { }

            return null;
        }
    }
}
