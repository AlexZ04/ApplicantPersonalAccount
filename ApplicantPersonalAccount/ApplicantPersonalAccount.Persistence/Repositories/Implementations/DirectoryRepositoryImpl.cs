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
            _directoryContext.EducationLevels.AddRange(levels);
            await _directoryContext.SaveChangesAsync();
        }

        public async Task SetDocumentTypes(List<DocumentType> types)
        {
            var educationLevels = await _directoryContext.EducationLevels.ToListAsync();

            var documentTypes = types.Select(model => new DocumentType
            {
                Id = model.Id,
                Name = model.Name,
                CreateTime = model.CreateTime,

                EducationLevel = educationLevels.First(l => l.Id == model.EducationLevel.Id),

                NextEducationLevels = model.NextEducationLevels
                    .Select(l => educationLevels.First(lev => lev.Id == l.Id))
                    .ToList()
            }).ToList();

            _directoryContext.DocumentTypes.AddRange(documentTypes);
            await _directoryContext.SaveChangesAsync();
        }

        public async Task SetFaculties(List<Faculty> faculties)
        {
            _directoryContext.Faculties.AddRange(faculties);
            await _directoryContext.SaveChangesAsync();
        }

        public async Task AddEducationPrograms(List<EducationProgram> programs)
        {
            var educationLevels = await _directoryContext.EducationLevels.ToListAsync();
            var faculties = await _directoryContext.Faculties.ToListAsync();

            var educationPrograms = programs.Select(model => new EducationProgram
            {
                Id = model.Id,
                Name = model.Name,
                CreateTime = model.CreateTime,
                Code = model.Code,
                EducationForm = model.EducationForm,
                Language = model.Language,

                Faculty = faculties.First(f => f.Id == model.Faculty.Id),

                EducationLevel = educationLevels.First(l => l.Id == model.EducationLevel.Id)

            }).ToList();

            _directoryContext.EducationPrograms.AddRange(educationPrograms);
            await _directoryContext.SaveChangesAsync();
        }

        public async Task ResetAll()
        {
            var allPrograms = await _directoryContext.EducationPrograms.ToListAsync();
            _directoryContext.EducationPrograms.RemoveRange(allPrograms);
            await _directoryContext.SaveChangesAsync();

            var allDocumentTypes = await _directoryContext.DocumentTypes.ToListAsync();
            _directoryContext.DocumentTypes.RemoveRange(allDocumentTypes);
            await _directoryContext.SaveChangesAsync();

            var allEducationLevels = await _directoryContext.EducationLevels.ToListAsync();
            _directoryContext.EducationLevels.RemoveRange(allEducationLevels);
            await _directoryContext.SaveChangesAsync();

            var allFaculties = await _directoryContext.Faculties.ToListAsync();
            _directoryContext.Faculties.RemoveRange(allFaculties);
            await _directoryContext.SaveChangesAsync();
        }
    }
}
