using ApplicantPersonalAccount.Persistence.Repositories.Implementations;
using ApplicantPersonalAccount.Persistence.Repositories;

namespace ApplicantPersonalAccount.Applicant.Setup
{
    public static class SetupRepositories
    {
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IApplicationRepository, ApplicationRepositoryImpl>();
        }
    }
}
