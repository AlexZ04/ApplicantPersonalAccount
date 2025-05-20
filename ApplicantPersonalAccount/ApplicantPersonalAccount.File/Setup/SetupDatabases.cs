using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Document.Setup
{
    public class SetupDatabases
    {
        public static void AddDatabases(WebApplicationBuilder builder)
        {
            AddFilesDb(builder);
        }

        public static void AddFilesDb(WebApplicationBuilder builder)
        {
            var filesConnection = builder.Configuration.GetConnectionString("FilesConnection");

            builder.Services.AddDbContext<FileDataContext>(options => options.UseNpgsql(filesConnection));
        }

        public static void RunMigrations(WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var fileContext = serviceScope.ServiceProvider.GetService<FileDataContext>();

            fileContext?.Database.Migrate();
        }
    }
}
