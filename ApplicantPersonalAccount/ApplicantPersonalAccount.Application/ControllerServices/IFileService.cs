using ApplicantPersonalAccount.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ApplicantPersonalAccount.Application.ControllerServices
{
    public interface IFileService
    {
        public Task UploadFile(FileDocumentType documentType, IFormFile file, ClaimsPrincipal user);
        public Task DeleteFile(Guid id, ClaimsPrincipal user);
    }
}
