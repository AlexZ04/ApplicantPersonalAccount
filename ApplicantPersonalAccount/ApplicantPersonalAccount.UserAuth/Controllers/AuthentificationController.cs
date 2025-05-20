using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.Filters;
using ApplicantPersonalAccount.UserAuth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.UserAuth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthentificationController : ControllerBase
    {
        private readonly IAuthService _authorizationService;

        public AuthentificationController(IAuthService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel user)
        {
            var validationErrors = UserAuthValidator.ValidateUserRegister(user);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors });

            return Ok(await _authorizationService.RegisterUser(user));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginModel loginCredentials)
        {
            return Ok(await _authorizationService.LoginUser(loginCredentials));
        }

        [HttpPost("logout")]
        [Authorize]
        [CheckToken]
        public async Task<IActionResult> Logout()
        {
            await _authorizationService.Logout(HttpContext.GetTokenAsync("access_token").Result, User);

            return Ok();
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginRefresh([FromBody] RefreshTokenModel tokenModel)
        {
            return Ok(await _authorizationService.LoginRefresh(tokenModel));
        }
    }
}
