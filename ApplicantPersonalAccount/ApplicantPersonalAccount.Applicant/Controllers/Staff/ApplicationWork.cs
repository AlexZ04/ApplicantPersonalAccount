using ApplicantPersonalAccount.Applicant.Services;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.Applicant.Controllers.Staff
{
    public partial class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost("take-application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> TakeEnterance([Required, FromRoute] Guid id)
        {
            await _staffService.AttachEnteranceToManager(id, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpDelete("take-application/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> RefuceEnterance([Required, FromRoute] Guid id)
        {
            await _staffService.UnattachEnteranceFromManager(id);

            return Ok();
        }

        [HttpPost("managers")]
        [Authorize(Roles = "HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> AttachManager(
            [FromQuery, Required] Guid managerId,
            [FromQuery, Required] Guid userId
            )
        {
            await _staffService.AttachEnteranceToManager(userId, managerId);

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
