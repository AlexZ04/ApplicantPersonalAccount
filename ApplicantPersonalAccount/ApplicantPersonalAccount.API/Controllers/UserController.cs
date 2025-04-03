using ApplicantPersonalAccount.Application;
using ApplicantPersonalAccount.Common.Models.User;
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
        public async Task<IActionResult> ChangePassword([FromBody] PasswordEditModel passwordModel)
        {
            await _userService.ChangePassword(passwordModel, User);

            return Ok();
        }

        [HttpPatch("change-email")]
        [Authorize(Roles = "Manager,HeadManager,Admin")]
        public async Task<IActionResult> ChangeEmail([FromBody] EmailEditModel emailModel)
        {
            await _userService.ChangeEmail(emailModel, User);

            return Ok();
        }

        [HttpPut("edit-profile")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> EditProfile([FromBody] UserEditModel userNewInfo)
        {
            await _userService.EditProfile(userNewInfo, User);

            return Ok();
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _userService.GetProfile(User));
        }
    }
}
