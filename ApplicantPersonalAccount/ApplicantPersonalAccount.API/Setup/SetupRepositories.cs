using ApplicantPersonalAccount.Persistence.Repositories.Implementations;
using ApplicantPersonalAccount.Persistence.Repositories;

namespace ApplicantPersonalAccount.API.Setup
{
    public static class SetupRepositories
    {
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepositoryImpl>();
            services.AddScoped<ITokenRepository, TokenRepositoryImpl>();
        }
    }
}
