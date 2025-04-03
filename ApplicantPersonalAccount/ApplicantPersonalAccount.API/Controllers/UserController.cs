using ApplicantPersonalAccount.Application;
using ApplicantPersonalAccount.Common.Models.User;
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
        public async Task<IActionResult> ChangePassword([FromBody] PasswordEditModel passwordModel)
        {
            await _userService.ChangePassword(passwordModel, User);

            return Ok();
        }

        [HttpPatch("change-email")]
        public async Task<IActionResult> ChangeEmail([FromBody] EmailEditModel emailModel)
        {
            await _userService.ChangeEmail(emailModel, User);

            return Ok();
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _userService.GetProfile(User));
        }
    }
}
