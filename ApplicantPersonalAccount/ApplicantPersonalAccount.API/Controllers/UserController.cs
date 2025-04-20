using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Common.Models.User;
using ApplicantPersonalAccount.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.API.Controllers
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
            await _userService.ChangePassword(passwordModel, User);

            return Ok();
        }

        [HttpPatch("change-email")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        [CheckToken]
        public async Task<IActionResult> ChangeEmail([FromBody] EmailEditModel emailModel)
        {
            await _userService.ChangeEmail(emailModel, User);

            return Ok();
        }

        [HttpPut("edit-profile")]
        [Authorize(Roles = "Applicant")]
        [CheckToken]
        public async Task<IActionResult> EditProfile([FromBody] UserEditModel userNewInfo)
        {
            await _userService.EditProfile(userNewInfo, User);

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
