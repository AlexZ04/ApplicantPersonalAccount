using ApplicantPersonalAccount.Applicant.Services;
using ApplicantPersonalAccount.Applicant.Validators;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Applicant.Controllers
{
    [ApiController]
    [Route("api/applicant")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        private readonly IApplicationService _applicationService;
        private readonly IDirectoryHelperService _directoryHelperService;
        private readonly IEnteranceService _enteranceService;

        public ApplicantController(
            IApplicantService applicantService,
            IApplicationService applicationService,
            IDirectoryHelperService directoryHelperService,
            IEnteranceService enteranceService)
        {
            _applicantService = applicantService;
            _applicationService = applicationService;
            _directoryHelperService = directoryHelperService;
            _enteranceService = enteranceService;
        }

        [HttpPost("notifications")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> SignToNotification()
        {
            await _applicantService.SignToNotifications(UserDescriptor.GetUserEmail(User));

            return Ok();
        }

        [HttpDelete("notifications")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> UnsignFromNotifications()
        {
            await _applicantService.UnsignFromNotifications(UserDescriptor.GetUserEmail(User));

            return Ok();
        }

        [HttpGet("education-programs")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetListOfPrograms(
            [FromQuery] string? faculty,
            [FromQuery] string? educationForm,
            [FromQuery] string? language,
            [FromQuery] string? code,
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            var validationErrors = PaginationValidator.ValidatePagination(page, size);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            return Ok(await _directoryHelperService.GetListOfPrograms(
                faculty != null ? faculty.ToLower() : "",
                educationForm != null ? educationForm.ToLower() : "",
                language != null ? language.ToLower() : "",
                code != null ? code.ToLower() : "",
                name != null ? name.ToLower() : "",
                page,
                size));
        }

        [HttpGet("document-types")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetDocumentTypes()
        {
            return Ok(await _directoryHelperService.GetDocumentTypes());
        }

        [HttpGet("info-for-events")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> GetInfoForEvents()
        {
            return Ok(await _applicantService.GetInfoForEvents(UserDescriptor.GetUserId(User)));
        }

        [HttpPut("info-for-events")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public IActionResult EditInfoForEvents([FromBody] EditApplicantInfoForEventsModel editedInfo)
        {
            _applicantService.EditInfoForEvents(editedInfo, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpPost("program")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> AddProgram([FromBody] EducationProgramApplicationModel program)
        {
            var validationErrors = EnteranceValidator.ValidateEnterancePriority(program.Priority);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            await _applicationService.AddProgram(program, UserDescriptor.GetUserId(User), UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpPut("program/{programId}")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> EditProgram([FromBody] EducationProgramApplicationEditModel program,
            [FromRoute] Guid programId)
        {
            var validationErrors = EnteranceValidator.ValidateEnterancePriority(program.Priority);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            await _applicationService.EditProgram(program, programId,
                UserDescriptor.GetUserId(User), UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpDelete("program/{programId}")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> DeleteProgram([FromRoute] Guid programId)
        {
            await _applicationService.DeleteProgram(programId, 
                UserDescriptor.GetUserId(User), UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpGet("program")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> GetEnteranceInfo()
        {
            return Ok(await _enteranceService.GetEnteranceByUserId(UserDescriptor.GetUserId(User)));
        }
    }
}
