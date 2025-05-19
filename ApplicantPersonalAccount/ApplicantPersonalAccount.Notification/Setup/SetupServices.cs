using ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Notification.MessageListener;
using ApplicantPersonalAccount.Notification.Services;
using ApplicantPersonalAccount.Notification.Services.Implementations;

namespace ApplicantPersonalAccount.Notification.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationServiceImpl>();

            services.AddHostedService<NotificationListener>();
            services.AddHostedService<SubscribtionsToNotifListener>();
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessageProducer, RabbitMqProducer>();
        }
    }
}
