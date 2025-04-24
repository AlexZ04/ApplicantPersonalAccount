using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.API.Controllers
{
    [Route("api/appliant")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpGet("programs")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetListOfPrograms(
            [FromQuery] string faculty, 
            [FromQuery] string educationForm,
            [FromQuery] string language,
            [FromQuery] string code,
            [FromQuery] string name,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            var validationErrors = Validator.Validator.ValidatePagination(page, size);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            return Ok(await _applicantService.GetListOfPrograms(faculty, educationForm, language, code, name, page, size));
        }

        [HttpPost("notifications")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> SignToNotification()
        {
            await _applicantService.SignToNotifications(UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpDelete("notifications")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> UnsighFromNotifications()
        {
            await _applicantService.UnsignFromNotifications(UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpPut("info-for-events")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> EditInfoForEvents([FromBody] EditApplicantInfoForEventsModel editedInfo)
        {
            await _applicantService.EditInfoForEvents(editedInfo, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpGet("info-for-events")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> GetInfoForEvents()
        {
            return Ok(await _applicantService.GetInfoForEvents(UserDescriptor.GetUserId(User)));
        }

        [HttpPost("program")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> AddProgram([FromBody] EducationProgramApplicationModel program)
        {
            await _applicantService.AddProgram(program, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpPut("program/{programId}")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> EditProgram([FromBody] EducationProgramApplicationModel program,
            [FromRoute] Guid programId)
        {
            await _applicantService.EditProgram(program, programId, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpDelete("program/{programId}")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> DeleteProgram([FromRoute] Guid programId)
        {
            await _applicantService.DeleteProgram(programId, UserDescriptor.GetUserId(User));

            return Ok();
        }
    }
}
