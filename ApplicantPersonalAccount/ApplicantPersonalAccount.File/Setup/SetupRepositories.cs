using ApplicantPersonalAccount.Persistence.Repositories;
using ApplicantPersonalAccount.Persistence.Repositories.Implementations;

namespace ApplicantPersonalAccount.Document.Setup
{
    public class SetupRepositories
    {
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IDocumentRepository, DocumentRepositoryImpl>();
        }
    }
}
