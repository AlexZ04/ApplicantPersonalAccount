using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Persistence.Repositories;
using RabbitMQ.Client.Events;

namespace ApplicantPersonalAccount.UserAuth.MessageListener
{
    public class EditInfoEventsListener : BaseMessageListener<BrokerEditInfoForEventsDTO>
    {
        public EditInfoEventsListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.NOTIFICATION) { }

        protected override async Task<string?> ProcessMessage(
            BrokerEditInfoForEventsDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var userRep = serviceProvider.GetRequiredService<IUserRepository>();

            await userRep.EditInfoForEvents(message.Model, message.UserId);

            return null;
        }
    }
}
