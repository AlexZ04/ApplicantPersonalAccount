using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.DTOs;

namespace ApplicantPersonalAccount.DirectoryService.Services
{
    public interface IDirectoryInfoService
    {
        public Task<List<DocumentType>> GetDocumentTypes();
        public Task<ProgramPagedList> GetListOfPrograms(
            string faculty,
            string educationForm,
            string language,
            string code,
            string name,
            int page = 1,
            int size = 5);
        public Task<ProgramPagedList> GetListOfPrograms(GetProgramsDTO message);
    }
}
