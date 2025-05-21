using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.DirectoryService.DTOs;

namespace ApplicantPersonalAccount.DirectoryService.Services
{
    public interface IDirectoryLoadingService
    {
        public LoadingStatusDTO GetLoadingStatus();
        public Task<DocumentType> GetDocumentTypeById(Guid id);
        public Task RequestDictImport(DirectoryImportType importType);
    }
}
