using ApplicantPersonalAccount.Infrastructure.Filters;
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
        [HttpGet("program/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetProgramInfo()
        {
            return Ok();
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
