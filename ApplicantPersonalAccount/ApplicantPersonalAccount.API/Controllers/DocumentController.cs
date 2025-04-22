using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Models.Document;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.API.Controllers
{
    [Route("api/document")]
    [ApiController]
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
            var validationErrors = Validator.Validator.ValidateFile(file);

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

        [HttpGet("download/{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> DownloadFile([FromRoute] Guid id)
        {
            var fileInfo = await _fileService.GetFile(id);

            return File(fileInfo.FileContents, fileInfo.ContentType, fileInfo.FileDownloadName);
        }

        [HttpPut("passport")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> EditPassportInfo([FromBody] PassportInfoEditModel passport)
        {
            await _fileService.EditPassport(passport, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpPut("education")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> EditEducationInfo([FromBody] EducationInfoModel education,
            [FromQuery] Guid documentId)
        {
            return Ok();
        }
    }
}
