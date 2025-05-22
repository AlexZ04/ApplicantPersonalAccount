using ApplicantPersonalAccount.Notification.Models;
using ApplicantPersonalAccount.Notification.Services;
using RabbitMQ.Client.Events;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageListener;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;

namespace ApplicantPersonalAccount.Notification.MessageListener
{
    public class NotificationListener : BaseMessageListener<NotificationModel>
    {
        public NotificationListener(IServiceProvider serviceProvider, IConfiguration config) 
            : base(serviceProvider, config, RabbitQueues.NOTIFICATION) { }

        protected override async Task<string?> ProcessMessage(
            NotificationModel message,
            BasicDeliverEventArgs eventArgs,
            IServiceProvider serviceProvider)
        {
            var notificationService = serviceProvider.GetRequiredService<INotificationService>();

            await notificationService.SendEmail(NotificationsOptions.NOTIFICATION_KEY,
                message!);

            return null;
        }
        
    }
}
