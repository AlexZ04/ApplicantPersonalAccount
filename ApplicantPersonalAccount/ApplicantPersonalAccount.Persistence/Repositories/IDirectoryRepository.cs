using ApplicantPersonalAccount.Application.OuterServices.DTO;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface IDirectoryRepository
    {
        public Task SetEducationLevels(List<EducationLevel> levels);
        public Task SetDocumentTypes(List<DocumentType> types);
        public Task SetFaculties(List<Faculty> faculties);
        public Task ResetAll();
        public Task SetEducationPrograms(List<EducationProgram> programs);
    }
}
