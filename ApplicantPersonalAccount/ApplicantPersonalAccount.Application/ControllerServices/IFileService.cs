using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Document;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Application.ControllerServices
{
    public interface IFileService
    {
        public Task UploadFile(FileDocumentType documentType, IFormFile file, Guid userId);
        public Task DeleteFile(Guid id, Guid userId);
        public Task<DocumentModel> GetDocumentInfo(Guid id);
        public Task<FileContentResult> GetFile(Guid id);
        public Task<List<DocumentModel>> GetUserDocuments(FileDocumentType documentType, 
            Guid userId,
            bool importingAll = false);
        public Task EditPassport(PassportInfoEditModel editedPassport, Guid userId);
    }
}
