using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Applicant.Setup
{
    public static class SetupDatabases
    {
        public static void AddDatabases(WebApplicationBuilder builder)
        {
            AddApplicationsDb(builder);
            AddRedis(builder);
        }

        public static void AddApplicationsDb(WebApplicationBuilder builder)
        {
            var applicationsConnection = builder.Configuration.GetConnectionString("ApplicationsConnection");

            builder.Services.AddDbContext<ApplicationDataContext>(options => options.UseNpgsql(applicationsConnection));
        }

        public static void AddRedis(WebApplicationBuilder builder)
        {
            builder.Services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost";
                options.InstanceName = "ApplicantAPI";
            });
        }

        public static void RunMigrations(WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var applicationContext = serviceScope.ServiceProvider.GetService<ApplicationDataContext>();

            applicationContext?.Database.Migrate();
        }
    }
}
