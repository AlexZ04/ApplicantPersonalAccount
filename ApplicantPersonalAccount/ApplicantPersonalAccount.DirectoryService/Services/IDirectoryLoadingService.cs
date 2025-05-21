using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.DirectoryService.Services
{
    public interface IDirectoryLoadingService
    {
        public LoadingStatusDTO GetLoadingStatus();
        public Task<DocumentType> GetDocumentTypeById(Guid id);
        public Task RequestDictImport(DirectoryImportType importType);
    }
}
