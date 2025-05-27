using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Repositories;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class GetUserByIdListener : BaseMessageListener<Guid>
    {
        public GetUserByIdListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.GET_USER_BY_ID) { }

        protected override async Task<string?> ProcessMessage(
            Guid message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var userRep = serviceProvider.GetRequiredService<IUserRepository>();

            var userData = await userRep.GetUserById(message);

            return JsonSerializer.Serialize(userData);
        }
    }
}
