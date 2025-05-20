using ApplicantPersonalAccount.UserAuth.Services;
using ApplicantPersonalAccount.UserAuth.Services.Implementations;

namespace ApplicantPersonalAccount.UserAuth.Setup
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthServiceImpl>();
            services.AddTransient<ITokenService, TokenServiceImpl>();
            services.AddTransient<IUserService, UserServiceImpl>();
        }
    }
}
