using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Application.ControllerServices.Implementations;
using ApplicantPersonalAccount.Application.OuterServices;
using ApplicantPersonalAccount.Application.OuterServices.Implementations;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;

namespace ApplicantPersonalAccount.API.Setup
{
    public static class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthorizationServiceImpl>();
            services.AddTransient<ITokenService, TokenServiceImpl>();
            services.AddTransient<IUserService, UserServiseImpl>();
            services.AddTransient<IApplicantService, ApplicantServiceImpl>();
            services.AddTransient<IFileService, FileServiceImpl>();
            services.AddHttpClient<IDirectoryService, DirectoryServiceImpl>();

            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessageProducer, RabbitMqProducer>();
        }
    }
}
