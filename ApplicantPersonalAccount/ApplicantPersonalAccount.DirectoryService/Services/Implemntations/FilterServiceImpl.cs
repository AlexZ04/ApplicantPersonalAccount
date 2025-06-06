using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.DirectoryService.Services.Implemntations
{
    public class FilterServiceImpl : IFilterService
    {
        private readonly DirectoryDataContext _directoryContext;

        public FilterServiceImpl(DirectoryDataContext directoryContext)
        {
            _directoryContext = directoryContext;
        }

        public async Task<List<Guid>> GetFilteredPrograms(string program, List<string> faculties)
        {
            var idQuery = _directoryContext.EducationPrograms
                .Include(p => p.Faculty)
                .Where(p => p.Name.ToLower().Contains(program.ToLower()));

            if (faculties.Count > 0)
                idQuery = idQuery.Where(p => faculties.Any(f => p.Faculty.Name.ToLower().Contains(f.ToLower())));

            var ids = await idQuery
                .Select(p => p.Id)
                .ToListAsync();

            return ids;
        }
    }
}
