using ApplicantPersonalAccount.Application.ControllerServices;
using ApplicantPersonalAccount.Common.Models.Authorization;
using ApplicantPersonalAccount.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthService _authorizationService;
        private readonly ITokenService _tokenService;

        public AuthorizationController(IAuthService authorizationService, ITokenService tokenService)
        {
            _authorizationService = authorizationService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel user)
        {
            var validationErrors = Validator.Validator.ValidateUserRegister(user);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors});

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
