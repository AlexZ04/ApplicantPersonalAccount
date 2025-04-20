using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Repositories.Implementations
{
    public class DirectoryRepositoryImpl : IDirectoryRepository
    {
        private readonly DirectoryDataContext _directoryContext;

        public DirectoryRepositoryImpl(DirectoryDataContext directoryContext)
        {
            _directoryContext = directoryContext;
        }

        public async Task SetEducationLevels(List<EducationLevel> levels)
        {
            var allLevels = await _directoryContext.EducationLevels.ToListAsync();
            _directoryContext.EducationLevels.RemoveRange(allLevels);
            _directoryContext.EducationLevels.AddRange(levels);

            await _directoryContext.SaveChangesAsync();
        }

        public async Task SetDocumentTypes(List<DocumentType> types)
        {
            var allTypes = await _directoryContext.DocumentTypes.ToListAsync();
            _directoryContext.DocumentTypes.RemoveRange(allTypes);
            _directoryContext.DocumentTypes.AddRange(types);

            await _directoryContext.SaveChangesAsync();
        }

        public async Task SetFaculties(List<Faculty> faculties)
        {
            var allFaculties = await _directoryContext.Faculties.ToListAsync();
            _directoryContext.Faculties.RemoveRange(allFaculties);
            _directoryContext.Faculties.AddRange(faculties);

            await _directoryContext.SaveChangesAsync();
        }

        public async Task AddEducationPrograms(List<EducationProgram> programs)
        {
            _directoryContext.EducationPrograms.AddRange(programs);
            await _directoryContext.SaveChangesAsync();
        }

        public async Task ResetEducationPrograms()
        {
            var allPrograms = await _directoryContext.EducationPrograms.ToListAsync();
            _directoryContext.EducationPrograms.RemoveRange(allPrograms);
            await _directoryContext.SaveChangesAsync();
        }
    }
}
