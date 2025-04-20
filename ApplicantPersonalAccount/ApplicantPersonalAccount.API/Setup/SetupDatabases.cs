using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.API.Setup
{
    public static class SetupDatabases
    {
        public static void AddDatabases(WebApplicationBuilder builder)
        {
            AddUserDB(builder);
            AddDirectoryDb(builder);
            AddRedis(builder);
        }

        public static void AddUserDB(WebApplicationBuilder builder)
        {
            var usersConnection = builder.Configuration.GetConnectionString("UsersConnection");

            builder.Services.AddDbContext<UserDataContext>(options => options.UseNpgsql(usersConnection));
        }

        public static void AddDirectoryDb(WebApplicationBuilder builder)
        {
            var directoryConnection = builder.Configuration.GetConnectionString("DirectoryConnection");

            builder.Services.AddDbContext<DirectoryDataContext>(options => options.UseNpgsql(directoryConnection));
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

            var userContext = serviceScope.ServiceProvider.GetService<UserDataContext>();
            var directoryContext = serviceScope.ServiceProvider.GetService<DirectoryDataContext>();

            userContext?.Database.Migrate();
            directoryContext?.Database.Migrate();
        }
    }
}
