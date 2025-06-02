using ApplicantPersonalAccount.Infrastructure.RabbitMq.Connection;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Staff.Domain.Services;
using ApplicantPersonalAccount.Staff.Domain.Services.Implementations;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

namespace ApplicantPersonalAccount.Staff.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessageProducer, RabbitMqProducer>();

            services.AddTransient<IStaffAuthService, StaffAuthServiceImpl>();
            services.AddTransient<IAdminDirectoryService, AdminDirectoryServiceImpl>();
            services.AddHttpContextAccessor();

            services.AddTransient<ServiceStorage, ServiceStorage>();
        }
    }
}
