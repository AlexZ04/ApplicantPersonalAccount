using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.UserAuth.Services;
using RabbitMQ.Client.Events;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class UpdateManagerListener : BaseMessageListener<ManagerUpdateDTO>
    {
        public UpdateManagerListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.UPDATE_MANAGER) { }

        protected override async Task<string?> ProcessMessage(
            ManagerUpdateDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var managerService = serviceProvider.GetRequiredService<IManagerService>();

            await managerService.UpdateManager(message);

            return null;
        }
    }
}
