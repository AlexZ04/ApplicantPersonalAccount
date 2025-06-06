using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Document.Services;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Document.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IFileService _fileService;

        public StaffController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("file/{userId}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> UploadFileToApplicant(
            [Required, FromRoute] Guid userId,
            [FromQuery, Required] FileDocumentType documentType,
            IFormFile file)
        {
            var validationErrors = FileControllerValidator.ValidateFile(file);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            await _fileService.UploadFile(documentType, file, userId);

            return Ok();
        }

        [HttpDelete("file/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> DeleteApplicantFile([Required, FromRoute] Guid id)
        {
            await _fileService.DeleteFile(id, UserDescriptor.GetUserId(User), UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpGet("files/{userId}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetUserFiles([Required, FromRoute] Guid userId, 
            [FromQuery, Required] FileDocumentType documentType)
        {
            return Ok(await _fileService.GetUserDocuments(documentType, userId));
        }

        [HttpGet("download/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> DownloadFile([FromRoute] Guid id)
        {
            var fileInfo = await _fileService.GetFile(id,
                UserDescriptor.GetUserId(User), UserDescriptor.GetUserRole(User));

            return File(fileInfo.FileContents, fileInfo.ContentType, fileInfo.FileDownloadName);
        }

        [HttpPut("passport/{userId}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> EditPassportInfo([Required, FromRoute] Guid userId, 
            [FromBody] PassportInfoEditModel passport)
        {
            await _fileService.EditPassport(passport, userId,
                UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpPut("education/{userId}/{documentId}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> EditEducationInfo([Required, FromRoute] Guid userId,
            [FromBody] EducationInfoEditModel education,
            [Required, FromRoute] Guid documentId)
        {
            await _fileService.EditEducational(education, documentId,
                userId, UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpGet("education/{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetEducationFileInfo([FromRoute] Guid id)
        {
            return Ok(await _fileService.GetEducationDocumentInfo(id,
                UserDescriptor.GetUserId(User), UserDescriptor.GetUserRole(User)));
        }

        [HttpGet("passport/{userId}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetUserPassportInfo([FromRoute] Guid userId)
        {
            return Ok(await _fileService.GetPassportInfo(userId));
        }
    }
}
