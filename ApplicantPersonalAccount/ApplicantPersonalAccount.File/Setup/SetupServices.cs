using ApplicantPersonalAccount.File.Services;
using ApplicantPersonalAccount.File.Services.Implementations;

namespace ApplicantPersonalAccount.File.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IFileService, FileServiceImpl>();
        }
    }
}
