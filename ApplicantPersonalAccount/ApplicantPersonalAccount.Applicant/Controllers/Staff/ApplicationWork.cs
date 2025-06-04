using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Applicant.Controllers.Staff
{
    public partial class StaffController : ControllerBase
    {
        [HttpPost("take-application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> TakeEnterance([Required, FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpDelete("take-application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> RefuceEnterance([Required, FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpPost("managers")]
        [Authorize(Roles = "HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> AttachManager(
            [FromQuery, Required] Guid managerId,
            [FromQuery, Required] Guid appliacationtId
            )
        {
            return Ok();
        }

        [HttpGet("applications")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetPrograms(
            [FromQuery] string name,
            [FromQuery] string program,
            [FromQuery] List<string> faculty,
            [FromQuery] EnteranceStatus status,
            [FromQuery] bool hasManagerOnly,
            [FromQuery] bool attachedToManager,
            [FromQuery] SortingType sortedByUpdateDate,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            return Ok();
        }
    }
}
