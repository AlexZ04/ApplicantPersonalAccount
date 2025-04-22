using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Infrastructure.Filters;
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

        [HttpPost("upload")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> UploadFile([FromQuery, Required] FileDocumentType documentType, IFormFile file)
        {
            var validationErrors = Validator.Validator.ValidateFile(file);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            await _fileService.UploadFile(documentType, file, User);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> DeleteFile([FromRoute] Guid id)
        {
            await _fileService.DeleteFile(id, User);

            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetFileInfo([FromRoute] Guid id)
        {
            return Ok(await _fileService.GetDocumentInfo(id));
        }
    }
}
