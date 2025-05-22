using ApplicantPersonalAccount.Applicant.Services;
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

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpPost("notifications")]
        //[Authorize(Roles = "Applicant")]
        //[CheckToken]
        public async Task<IActionResult> SignToNotification()
        {
            //await _applicantService.SignToNotifications(UserDescriptor.GetUserId(User));
            await _applicantService.SignToNotifications(Guid.NewGuid());

            return Ok();
        }

        [HttpDelete("notifications")]
        //[Authorize(Roles = "Applicant")]
        //[CheckToken]
        public async Task<IActionResult> UnsighFromNotifications()
        {
            //await _applicantService.UnsignFromNotifications(UserDescriptor.GetUserId(User));
            await _applicantService.UnsignFromNotifications(Guid.NewGuid());

            return Ok();
        }
    }
}
