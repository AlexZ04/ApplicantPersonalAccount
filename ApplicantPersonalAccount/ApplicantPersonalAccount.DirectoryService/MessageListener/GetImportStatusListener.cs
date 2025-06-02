using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using RabbitMQ.Client.Events;

namespace ApplicantPersonalAccount.DirectoryService.MessageListener
{
    public class GetImportStatusListener : BaseMessageListener<BrokerRequestDTO>
    {
        public GetImportStatusListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.IMPORT_STATUS) { }

        protected override async Task<string?> ProcessMessage(
            BrokerRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            return ImportStatusHolder.ImportStatus;
        }
    }
}
