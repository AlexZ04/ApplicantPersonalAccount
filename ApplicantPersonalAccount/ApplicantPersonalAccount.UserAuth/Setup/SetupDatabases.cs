using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.UserAuth.Setup
{
    public class SetupDatabases
    {
        public static void AddDatabases(WebApplicationBuilder builder)
        {
            AddUsersDb(builder);
            AddRedis(builder);
        }

        public static void AddUsersDb(WebApplicationBuilder builder)
        {
            var usersConnection = builder.Configuration.GetConnectionString("UsersConnection");

            builder.Services.AddDbContext<UserDataContext>(options => options.UseNpgsql(usersConnection));
        }

        public static void AddRedis(WebApplicationBuilder builder)
        {
            builder.Services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost:6379";
                options.InstanceName = "ApplicantAPI";
            });
        }

        public static void RunMigrations(WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var userContext = serviceScope.ServiceProvider.GetService<UserDataContext>();

            userContext?.Database.Migrate();
        }
    }
}
