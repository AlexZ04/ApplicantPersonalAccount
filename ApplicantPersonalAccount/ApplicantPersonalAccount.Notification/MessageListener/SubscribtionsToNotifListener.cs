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
                await notificationService.SignUserToNotifications(message.UserEmail);
            else
                await notificationService.UnsignUserFromNotifications(message.UserEmail);

            return null;
        }
    }
}
