using ApplicantPersonalAccount.Persistence.Repositories;
using ApplicantPersonalAccount.Persistence.Repositories.Implementations;

namespace ApplicantPersonalAccount.UserAuth.Setup
{
    public class SetupRepositories
    {
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ITokenRepository, TokenRepositoryImpl>();
            services.AddScoped<IUserRepository, UserRepositoryImpl>();
        }
    }
}
