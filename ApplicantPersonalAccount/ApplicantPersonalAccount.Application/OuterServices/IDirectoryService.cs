using ApplicantPersonalAccount.Application.OuterServices.DTO;

namespace ApplicantPersonalAccount.Application.OuterServices
{
    public interface IDirectoryService
    {
        public Task<List<EducationLevel>> GetEducationLevels();
        public Task<List<DocumentType>> GetDocumentTypes();
        public Task<List<Faculty>> GetFaculties();
        public Task<ProgramPagedList> GetEducationPrograms(int page = 1, int size = 10);
    }
}
