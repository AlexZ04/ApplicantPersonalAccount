using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.DirectoryService.Setup
{
    public class SetupDatabases
    {
        public static void AddDatabases(WebApplicationBuilder builder)
        {
            AddDirectoryDb(builder);
            AddRedis(builder);
        }

        public static void AddDirectoryDb(WebApplicationBuilder builder)
        {
            var usersConnection = builder.Configuration.GetConnectionString("DirectoryConnection");

            builder.Services.AddDbContext<UserDataContext>(options => options.UseNpgsql(usersConnection));
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

            var directoryContext = serviceScope.ServiceProvider.GetService<DirectoryDataContext>();

            directoryContext?.Database.Migrate();
        }
    }
}
