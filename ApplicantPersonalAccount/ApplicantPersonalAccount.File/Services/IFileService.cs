using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Common.Models.Document;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Document.Services
{
    public interface IFileService
    {
        public Task UploadFile(FileDocumentType documentType, IFormFile file, Guid userId);
        public Task DeleteFile(Guid id, Guid userId, string userRole);
        public Task<DocumentModel> GetDocumentInfo(Guid id);
        public Task<FileContentResult> GetFile(Guid id, Guid userId, string userRole);
        public Task<List<DocumentModel>> GetUserDocuments(FileDocumentType documentType,
            Guid userId,
            bool importingAll = false);
        public Task EditPassport(PassportInfoEditModel editedPassport, Guid userId, string userRole);
        public Task EditEducational(EducationInfoEditModel editedEducation, Guid documentId,
            Guid userId, string userRole);
        public Task<DocumentType> GetEducationDocumentInfo(Guid documentId, Guid userId, string userRole);
        public Task<PassportInfoModel> GetPassportInfo(Guid userId);
    }
}
