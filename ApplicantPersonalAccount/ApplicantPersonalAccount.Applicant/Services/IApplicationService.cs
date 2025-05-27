using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IApplicationService
    {
        public Task AddProgram(EducationProgramApplicationModel program, Guid userId, string userRole);
        public Task EditProgram(EducationProgramApplicationEditModel program, Guid programId,
            Guid userId, string userRole);
        public Task DeleteProgram(Guid programId, Guid userId, string userRole);
    }
}
