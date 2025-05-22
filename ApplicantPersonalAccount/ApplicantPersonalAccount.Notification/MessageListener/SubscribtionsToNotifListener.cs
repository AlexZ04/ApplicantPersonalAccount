using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Notification.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ApplicantPersonalAccount.Notification.MessageListener
{
    public class SubscribtionsToNotifListener : BaseMessageListener<SubscriptionToNotificationDTO>
    {
        public SubscribtionsToNotifListener(IServiceProvider serviceProvider, IConfiguration config)
            : base(serviceProvider, config, RabbitQueues.SUBS) { }

        protected override async Task<string?> ProcessMessage(
            SubscriptionToNotificationDTO message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var notificationService = serviceProvider.GetRequiredService<INotificationService>();

            if (message!.Subscribe)
                try
                {
                    await notificationService.SignUserToNotifications(message.UserEmail);

                    return BrokerMessages.USER_IS_SUCCESSFULY_SIGNED;
                }
                catch (Exception e)
                {
                    return e.Message;
                }

            try
            {
                await notificationService.UnsignUserFromNotifications(message.UserEmail);

                return BrokerMessages.USER_IS_SUCCESSFULY_UNSIGNED;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
