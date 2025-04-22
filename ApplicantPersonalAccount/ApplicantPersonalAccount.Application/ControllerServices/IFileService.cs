using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Document;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApplicantPersonalAccount.Application.ControllerServices
{
    public interface IFileService
    {
        public Task UploadFile(FileDocumentType documentType, IFormFile file, ClaimsPrincipal user);
        public Task DeleteFile(Guid id, ClaimsPrincipal user);
        public Task<DocumentModel> GetDocumentInfo(Guid id);
        public Task<FileContentResult> GetFile(Guid id);
        public Task<List<DocumentModel>> GetUserDocuments(FileDocumentType documentType, Guid userId);
    }
}
