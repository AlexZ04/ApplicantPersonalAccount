using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class ApplicationServiceImpl : IApplicationService
    {
        public Task AddProgram(EducationProgramApplicationModel program, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProgram(Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
