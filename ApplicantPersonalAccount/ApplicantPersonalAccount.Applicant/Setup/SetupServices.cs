using ApplicantPersonalAccount.Applicant.MessageListeners;
using ApplicantPersonalAccount.Applicant.Services;
using ApplicantPersonalAccount.Applicant.Services.Implementations;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;

namespace ApplicantPersonalAccount.Applicant.Setup
{
    public static class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessageProducer, RabbitMqProducer>();

            services.AddTransient<IApplicantService, ApplicantServiceImpl>();
            services.AddTransient<IApplicationService, ApplicationServiceImpl>();
            services.AddTransient<IDirectoryHelperService, DirectoryHelperServiceImpl>();
            services.AddTransient<IStaffService, StaffServiceImpl>();
            services.AddTransient<IEnteranceService, EnteranceServiceImpl>();

            services.AddHostedService<CanEditListener>();
            services.AddHostedService<CreateEnteranceListener>();
        }
    }
}
