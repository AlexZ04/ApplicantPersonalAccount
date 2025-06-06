using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Document.Services;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Document.Controllers
{
    [ApiController]
    [Route("api/document")]
    public class DocumentController : ControllerBase
    {
        private readonly IFileService _fileService;

        public DocumentController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetUserFiles([FromQuery, Required] FileDocumentType documentType)
        {
            return Ok(await _fileService.GetUserDocuments(documentType, UserDescriptor.GetUserId(User)));
        }

        [HttpPost("upload")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> UploadFile([FromQuery, Required] FileDocumentType documentType, IFormFile file)
        {
            var validationErrors = FileControllerValidator.ValidateFile(file);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            await _fileService.UploadFile(documentType, file, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> DeleteFile([FromRoute] Guid id)
        {
            await _fileService.DeleteFile(id, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetFileInfo([FromRoute] Guid id)
        {
            return Ok(await _fileService.GetDocumentInfo(id));
        }

        [HttpGet("passport")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetPassportInfo()
        {
            return Ok(await _fileService.GetPassportInfo(UserDescriptor.GetUserId(User)));
        }

        [HttpGet("education/{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetEducationFileInfo([FromRoute] Guid id)
        {
            return Ok(await _fileService.GetEducationDocumentInfo(id));
        }

        [HttpGet("download/{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> DownloadFile([FromRoute, Required] Guid id)
        {
            var fileInfo = await _fileService.GetFile(id);

            return File(fileInfo.FileContents, fileInfo.ContentType, fileInfo.FileDownloadName);
        }

        [HttpPut("passport")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> EditPassportInfo([FromBody] PassportInfoEditModel passport)
        {
            await _fileService.EditPassport(passport,
                UserDescriptor.GetUserId(User),
                UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpPut("education")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> EditEducationInfo([FromBody] EducationInfoEditModel education,
            [FromQuery, Required] Guid documentId)
        {
            await _fileService.EditEducational(education, documentId,
                UserDescriptor.GetUserId(User), UserDescriptor.GetUserRole(User));

            return Ok();
        }
    }
}
