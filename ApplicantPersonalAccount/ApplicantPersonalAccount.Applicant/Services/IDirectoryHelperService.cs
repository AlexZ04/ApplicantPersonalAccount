using ApplicantPersonalAccount.Application.OuterServices.DTO;

namespace ApplicantPersonalAccount.Applicant.Services
{
    public interface IDirectoryHelperService
    {
        public Task<ProgramPagedList> GetListOfPrograms(
            string faculty,
            string educationForm,
            string language,
            string code,
            string name,
            int page = 1,
            int size = 5);

        public Task<List<DocumentType>> GetDocumentTypes();
    }
}
