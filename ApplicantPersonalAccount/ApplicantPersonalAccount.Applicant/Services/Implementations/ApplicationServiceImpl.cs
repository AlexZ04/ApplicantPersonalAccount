using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Persistence.Repositories;
using ApplicantPersonalAccount.Persistence.Repositories.Implementations;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class ApplicationServiceImpl : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationServiceImpl(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task AddProgram(EducationProgramApplicationModel program, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProgram(Guid programId, Guid userId)
        {
            await _applicationRepository.DeleteProgram(programId, userId);
        }
    }
}
