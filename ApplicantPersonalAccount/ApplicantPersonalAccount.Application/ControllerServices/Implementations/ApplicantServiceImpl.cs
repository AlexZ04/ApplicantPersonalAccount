using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Application.OuterServices;
using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Models;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Application.ControllerServices.Implementations
{
    public class ApplicantServiceImpl : IApplicantService
    {
        private readonly DirectoryDataContext _directoryContext;

        public ApplicantServiceImpl(DirectoryDataContext directoryContext)
        {
            _directoryContext = directoryContext;
        }

        public async Task<ProgramPagedList> GetListOfPrograms(
            string faculty, 
            string educationForm, 
            string language, 
            string code, 
            string name, 
            int page = 1, 
            int size = 5)
        {
            name = name ?? string.Empty;
            var programs = _directoryContext.EducationPrograms
                .Include(p => p.EducationLevel)
                .Include(p => p.Faculty)
                .Where(p => p.Name.Contains(name));

            if (faculty != null)
                programs = programs.Where(p => p.Faculty.Name.Contains(faculty));

            if (language != null)
                programs = programs.Where(p => p.Language.Contains(language));

            if (code != null)
                programs = programs.Where(p => p.Code.Contains(code));

            if (educationForm != null)
                programs = programs.Where(p => p.EducationForm.Contains(educationForm));

            var totalCount = await programs.CountAsync();

            programs = programs
                .Skip((page - 1) * size)
                .Take(size);

            var pagination = new PageInfoModel
            {
                Current = page,
                Size = size,
                Count = (totalCount / size) + (totalCount % size > 0 ? 1 : 0)
            };

            var result = new ProgramPagedList
            {
                Programs = await programs.ToListAsync(),
                Pagination = pagination
            };

            return result;
        }
    }
}
