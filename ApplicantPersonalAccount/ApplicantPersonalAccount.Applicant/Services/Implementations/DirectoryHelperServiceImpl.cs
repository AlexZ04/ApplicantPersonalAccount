using ApplicantPersonalAccount.Application.OuterServices.DTO;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class DirectoryHelperServiceImpl : IDirectoryHelperService
    {
        public Task<List<DocumentType>> GetDocumentTypes()
        {
            throw new NotImplementedException();
        }

        public Task<ProgramPagedList> GetListOfPrograms(
            string faculty,
            string educationForm,
            string language,
            string code,
            string name,
            int page = 1,
            int size = 5)
        {
            throw new NotImplementedException();
        }
    }
}
