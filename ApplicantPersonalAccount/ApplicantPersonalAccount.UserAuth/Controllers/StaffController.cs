using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.UserAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplicantPersonalAccount.UserAuth.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IUserService _userService;

        public StaffController(
            IManagerService managerService,
            IUserService userService,
            ILogger<StaffController> logger)
        {
            _managerService = managerService;
            _userService = userService;
        }

        [HttpGet("managers")]
        [Authorize(Roles = "HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetManagersList()
        {
            return Ok(await _managerService.GetAllManagers());
        }

        [HttpGet("profile/{id}")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> GetUserProfile([FromRoute, Required] Guid id)
        {
            return Ok(await _userService.GetProfile(id));
        }
    }
}
