using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IApplicationService
    {
        public Task AddProgram(EducationProgramApplicationModel program, Guid userId);
        public Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId);
        public Task DeleteProgram(Guid programId, Guid userId);
    }
}
