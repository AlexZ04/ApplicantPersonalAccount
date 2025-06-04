using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.DTOs.Managers;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.UserAuth.Services;
using RabbitMQ.Client.Events;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class CreateManagerListener : BaseMessageListener<ManagerCreateDTO>
    {
        public CreateManagerListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.CREATE_MANGER) { }

        protected override async Task<string?> ProcessMessage(
            ManagerCreateDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var managerService = serviceProvider.GetRequiredService<IManagerService>();

            try
            {
                await managerService.CreateManager(message);
                return "created";
            }
            catch
            {
                return null;
            }
        }
    }
}
