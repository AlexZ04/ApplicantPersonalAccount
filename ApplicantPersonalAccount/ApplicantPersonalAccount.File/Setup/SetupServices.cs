using ApplicantPersonalAccount.Document.MessageListeners;
using ApplicantPersonalAccount.Document.Services;
using ApplicantPersonalAccount.Document.Services.Implementations;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;

namespace ApplicantPersonalAccount.Document.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IFileService, FileServiceImpl>();
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessageProducer, RabbitMqProducer>();

            services.AddHostedService<GetUserDocumentsListener>();
            services.AddHostedService<EducationInfoListener>();
        }
    }
}
