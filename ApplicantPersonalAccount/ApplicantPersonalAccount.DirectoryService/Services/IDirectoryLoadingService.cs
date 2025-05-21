using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.DirectoryService.Services
{
    public interface IDirectoryLoadingService
    {
        public Task<string> GetLoadingStatus();
        public Task<DocumentType> GetDocumentTypeById(Guid id);
        public Task RequestDictImport(DirectoryImportType importType);
    }
}
