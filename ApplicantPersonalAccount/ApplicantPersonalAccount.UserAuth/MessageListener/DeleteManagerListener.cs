using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.UserAuth.Services;
using RabbitMQ.Client.Events;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class DeleteManagerListener : BaseMessageListener<GuidRequestDTO>
    {
        public DeleteManagerListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.DELETE_MANAGER) { }

        protected override async Task<string?> ProcessMessage(
            GuidRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var managerService = serviceProvider.GetRequiredService<IManagerService>();

            await managerService.DeleteManagerById(message.Id);

            return null;
        }
    }
}
