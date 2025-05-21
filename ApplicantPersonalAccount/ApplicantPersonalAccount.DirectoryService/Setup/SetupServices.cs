using ApplicantPersonalAccount.DirectoryService.Services;
using ApplicantPersonalAccount.DirectoryService.Services.Implemntations;

namespace ApplicantPersonalAccount.DirectoryService.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddHttpClient<IDirectoryService, DirectoryServiceImpl>();
            services.AddTransient<IDirectoryLoadingService, DirectoryLoadingServiceImpl>();
        }
    }
}
