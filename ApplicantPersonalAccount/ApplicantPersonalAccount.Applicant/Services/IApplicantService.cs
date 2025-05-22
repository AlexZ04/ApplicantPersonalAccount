using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IApplicantService
    {
        public Task<ProgramPagedList> GetListOfPrograms(
            string faculty,
            string educationForm,
            string language,
            string code,
            string name,
            int page = 1,
            int size = 5);
        public Task SignToNotifications(string userEmail);
        public Task UnsignFromNotifications(string userEmail);
        public Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId);
        public Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId);
        public Task AddProgram(EducationProgramApplicationModel program, Guid userId);
        public Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId);
        public Task DeleteProgram(Guid programId, Guid userId);
        public Task<List<DocumentType>> GetDocumentTypes();
    }
}
