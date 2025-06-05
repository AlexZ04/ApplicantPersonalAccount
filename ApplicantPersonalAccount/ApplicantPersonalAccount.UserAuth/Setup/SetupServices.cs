using ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.UserAuth.MessageListener;
using ApplicantPersonalAccount.UserAuth.Services;
using ApplicantPersonalAccount.UserAuth.Services.Implementations;

namespace ApplicantPersonalAccount.UserAuth.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthServiceImpl>();
            services.AddTransient<ITokenService, TokenServiceImpl>();
            services.AddTransient<IUserService, UserServiceImpl>();
            services.AddTransient<IManagerService, ManagerServiceImpl>();
            services.AddTransient<IFilterService, FilterServiceImpl>();

            services.AddHostedService<GetEventsInfoListener>();
            services.AddHostedService<GetUserByIdListener>();
            services.AddHostedService<EditInfoEventsListener>();
            services.AddHostedService<LoginListener>();
            services.AddHostedService<RefreshLoginListener>();
            services.AddHostedService<LogoutListener>();
            services.AddHostedService<GetAllManagersListener>();
            services.AddHostedService<DeleteManagerListener>();
            services.AddHostedService<UpdateManagerListener>();
            services.AddHostedService<CreateManagerListener>();
            services.AddHostedService<FilterNameListener>();

            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessageProducer, RabbitMqProducer>();
        }
    }
}
