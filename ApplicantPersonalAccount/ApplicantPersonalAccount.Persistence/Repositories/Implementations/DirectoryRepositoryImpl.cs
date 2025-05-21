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
            foreach (var level in levels)
            {
                var foundRecord = await _directoryContext.EducationLevels
                    .FirstOrDefaultAsync(l => l.Name == level.Name);

                if (foundRecord == null)
                    _directoryContext.EducationLevels.Add(level);

            }

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

            foreach (var documentType in documentTypes)
            {
                var foundRecord = await _directoryContext.DocumentTypes
                    .FirstOrDefaultAsync(t => t.Name == documentType.Name);

                if (foundRecord == null)
                    _directoryContext.DocumentTypes.Add(documentType);
            }

            await _directoryContext.SaveChangesAsync();
        }

        public async Task SetFaculties(List<Faculty> faculties)
        {
            foreach (var faculty in faculties)
            {
                var foundRecord = await _directoryContext.Faculties
                    .FirstOrDefaultAsync(f => f.Name == faculty.Name);

                if (foundRecord == null)
                    _directoryContext.Faculties.Add(faculty);
            }

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

            foreach (var program in educationPrograms)
            {
                var foundRecord = await _directoryContext.EducationPrograms
                    .FirstOrDefaultAsync(p => p.Name == program.Name);

                if (foundRecord == null)
                    _directoryContext.EducationPrograms.Add(program);
            }

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
