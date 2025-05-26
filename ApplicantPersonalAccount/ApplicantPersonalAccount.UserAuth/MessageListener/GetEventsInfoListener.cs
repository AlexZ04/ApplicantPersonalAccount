using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Repositories;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class GetEventsInfoListener : BaseMessageListener<Guid>
    {
        public GetEventsInfoListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_INFO_FOR_EVENTS) { }

        protected override async Task<string?> ProcessMessage(
            Guid message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var userRep = serviceProvider.GetRequiredService<IUserRepository>();
            
            var userData = await userRep.GetInfoForEvents(message);

            return JsonSerializer.Serialize(userData);
        }
    }
}
