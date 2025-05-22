using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Models.Applicant;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class ApplicantServiceImpl : IApplicantService
    {
        public Task AddProgram(EducationProgramApplicationModel program, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProgram(Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task EditProgram(EducationProgramApplicationEditModel program, Guid programId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentType>> GetDocumentTypes()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantInfoForEventsModel> GetInfoForEvents(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ProgramPagedList> GetListOfPrograms(string faculty, string educationForm, string language, string code, string name, int page = 1, int size = 5)
        {
            throw new NotImplementedException();
        }

        public Task SignToNotifications(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UnsignFromNotifications(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
