using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Repositories;
using ApplicantPersonalAccount.UserAuth.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class GetAllManagersListener : BaseMessageListener<BrokerRequestDTO>
    {
        public GetAllManagersListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_ALL_MANAGERS) { }

        protected override async Task<string?> ProcessMessage(
            BrokerRequestDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var managerService = serviceProvider.GetRequiredService<IManagerService>();

            var managers = await managerService.GetAllManagers();

            return JsonSerializer.Serialize(managers);
        }
    }
}
