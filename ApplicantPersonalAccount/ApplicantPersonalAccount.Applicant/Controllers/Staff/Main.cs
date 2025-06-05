using ApplicantPersonalAccount.Applicant.Services;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Applicant.Controllers.Staff
{
    [Route("api/staff")]
    [ApiController]
    public partial class StaffController : ControllerBase
    {
        private readonly IEnteranceService _enteranceService;

        public StaffController(
            IEnteranceService enteranceService,
            IStaffService staffService)
        {
            _enteranceService = enteranceService;
            _staffService = staffService;
        }

        [HttpGet("program/{userId}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetProgramInfoByUserId([Required, FromRoute] Guid userId)
        {
            return Ok(await _enteranceService.GetEnteranceByUserId(userId));
        }

        [HttpGet("application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetApplicationInfo([Required, FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpPost("application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> EditApplicationInfo([Required, FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpDelete("application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> DeleteApplication([Required, FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpPatch("application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> UpdateApplicationStatus([Required, FromRoute] Guid id)
        {
            return Ok();
        }
    }
}
