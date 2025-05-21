using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Enums;

namespace ApplicantPersonalAccount.DirectoryService.Services.Implemntations
{
    public class DirectoryLoadingServiceImpl : IDirectoryLoadingService
    {
        public Task<DocumentType> GetDocumentTypeById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLoadingStatus()
        {
            throw new NotImplementedException();
        }

        public Task RequestDictImport(DirectoryImportType importType)
        {
            throw new NotImplementedException();
        }
    }
}
