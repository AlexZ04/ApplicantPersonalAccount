using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models;
using ApplicantPersonalAccount.Persistence.Contextes;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.DirectoryService.Services.Implemntations
{
    public class DirectoryInfoServiceImpl : IDirectoryInfoService
    {
        private readonly DirectoryDataContext _directoryContext;

        public DirectoryInfoServiceImpl(DirectoryDataContext directoryContext)
        {
            _directoryContext = directoryContext;
        }

        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            var documentTypes = await _directoryContext.DocumentTypes
                .Include(t => t.EducationLevel)
                .Include(t => t.NextEducationLevels)
                .ToListAsync();

            return documentTypes;
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
            if (_directoryContext.EducationPrograms.FirstOrDefault(e => e.Name.Contains("")) == null)
                throw new EarlyActionException(ErrorMessages.DICTIONARY_IS_NOT_LOADED);

            name = name ?? string.Empty;
            var programs = _directoryContext.EducationPrograms
                .Include(p => p.EducationLevel)
                .Include(p => p.Faculty)
                .AsNoTracking()
                .Where(p => p.Name.Contains(name));

            if (faculty != null)
                programs = programs.Where(p => p.Faculty.Name.ToLower().Contains(faculty));

            if (language != null)
                programs = programs.Where(p => p.Language.ToLower().Contains(language));

            if (code != null)
                programs = programs.Where(p => p.Code.ToLower().Contains(code));

            if (educationForm != null)
                programs = programs.Where(p => p.EducationForm.ToLower().Contains(educationForm));

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

        public async Task<ProgramPagedList> GetListOfPrograms(GetProgramsDTO message)
        {
            return await GetListOfPrograms(
                message.Faculty,
                message.EducationForm,
                message.Language,
                message.Code,
                message.Name,
                message.Page,
                message.Size);
        }

        public async Task<DocumentType> GetDocumentTypeById(Guid id)
        {
            var documentType = await _directoryContext.DocumentTypes
                .FirstOrDefaultAsync(t => t.Id == id);

            return documentType ?? throw new NotFoundException(ErrorMessages.DOCUMENT_TYPE_NOT_FOUND);
        }
    }
}
