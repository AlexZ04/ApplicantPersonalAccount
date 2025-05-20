using ApplicantPersonalAccount.Document.Services;
using ApplicantPersonalAccount.Document.Services.Implementations;

namespace ApplicantPersonalAccount.Document.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IFileService, FileServiceImpl>();
        }
    }
}
