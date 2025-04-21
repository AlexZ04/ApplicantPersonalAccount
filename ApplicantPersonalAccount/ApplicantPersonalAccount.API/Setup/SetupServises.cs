using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Application.ControllerServices.Implementations;
using ApplicantPersonalAccount.Application.OuterServices;
using ApplicantPersonalAccount.Application.OuterServices.Implementations;

namespace ApplicantPersonalAccount.API.Setup
{
    public static class SetupServises
    {
        public static void AddServises(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthorizationServiceImpl>();
            services.AddTransient<ITokenService, TokenServiceImpl>();
            services.AddTransient<IUserService, UserServiseImpl>();
            services.AddTransient<IApplicantService, ApplicantServiceImpl>();
            services.AddTransient<IFileService, FileServiceImpl>();
            services.AddHttpClient<IDirectoryService, DirectoryServiceImpl>();
        }
    }
}
