using ApplicantPersonalAccount.Common.Models.User;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.Infrastructure.Utilities;
using ApplicantPersonalAccount.UserAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApplicantPersonalAccount.UserAuth.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("change-password")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordEditModel passwordModel)
        {
            await _userService.ChangePassword(passwordModel, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpPatch("change-email")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> ChangeEmail([FromBody] EmailEditModel emailModel)
        {
            await _userService.ChangeEmail(emailModel, UserDescriptor.GetUserId(User));

            return Ok();
        }

        [HttpPut("edit-profile")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> EditProfile([FromBody] UserEditModel userNewInfo)
        {
            await _userService.EditProfile(userNewInfo, UserDescriptor.GetUserId(User),
                UserDescriptor.GetUserRole(User));

            return Ok();
        }

        [HttpGet("profile")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _userService.GetProfile(User));
        }
    }
}
