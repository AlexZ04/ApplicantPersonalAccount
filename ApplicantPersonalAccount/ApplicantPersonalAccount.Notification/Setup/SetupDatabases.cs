using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Notification.Setup
{
    public class SetupDatabases
    {
        public static void AddDatabases(WebApplicationBuilder builder)
        {
            var applicationsConnection = builder.Configuration.GetConnectionString("NotificationsConnection");

            builder.Services.AddDbContext<NotificationDataContext>(options => options.UseNpgsql(applicationsConnection));
        }

        public static void RunMigrations(WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var notificationContext = serviceScope.ServiceProvider.GetService<NotificationDataContext>();

            notificationContext?.Database.Migrate();
        }
    }
}
