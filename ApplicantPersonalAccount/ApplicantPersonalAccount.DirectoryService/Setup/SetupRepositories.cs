using ApplicantPersonalAccount.Persistence.Repositories;
using ApplicantPersonalAccount.Persistence.Repositories.Implementations;

namespace ApplicantPersonalAccount.DirectoryService.Setup
{
    public class SetupRepositories
    {
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IDirectoryRepository, DirectoryRepositoryImpl>();
        }
    }
}
