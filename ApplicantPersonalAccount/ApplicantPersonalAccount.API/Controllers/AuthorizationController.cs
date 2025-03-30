using ApplicantPersonalAccount.Application;
using ApplicantPersonalAccount.Common.Models.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel user)
        {
            var validationErrors = Validator.ValidateUserRegister(user);

            if (validationErrors.Count() > 0)
                return BadRequest(new { Errors = validationErrors});

            return Ok(await _authorizationService.RegisterUser(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginModel loginCredentials)
        {
            return Ok(await _authorizationService.LoginUser(loginCredentials));
        }

    }
}
