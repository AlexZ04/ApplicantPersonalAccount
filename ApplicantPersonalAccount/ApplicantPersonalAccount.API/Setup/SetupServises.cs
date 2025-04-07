using ApplicantPersonalAccount.Application.Implementations;
using ApplicantPersonalAccount.Application;

namespace ApplicantPersonalAccount.API.Setup
{
    public static class SetupServises
    {
        public static void AddServises(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthorizationServiceImpl>();
            services.AddTransient<ITokenService, TokenServiceImpl>();
            services.AddTransient<IUserService, UserServiseImpl>();
        }
    }
}
