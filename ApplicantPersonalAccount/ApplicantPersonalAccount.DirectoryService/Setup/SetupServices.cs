using ApplicantPersonalAccount.DirectoryService.MessageListener;
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
            services.AddTransient<IDirectoryInfoService, DirectoryInfoServiceImpl>();

            services.AddHostedService<DocumentTypeListener>();
            services.AddHostedService<ProgramListener>();
            services.AddHostedService<EducationProgramByIdListener>();
            services.AddHostedService<DocumentTypeByIdListener>();
            services.AddHostedService<GetImportStatusListener>();
            services.AddHostedService<RequestImportListener>();
        }
    }
}
