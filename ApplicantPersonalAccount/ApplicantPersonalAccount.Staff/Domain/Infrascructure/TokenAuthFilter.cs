using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantPersonalAccount.Staff.Domain.Infrascructure
{
    public class TokenAuthFilter : IAsyncAuthorizationFilter
    {
        private readonly IStaffAuthService _authService;

        public TokenAuthFilter(IStaffAuthService authService)
        {
            _authService = authService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.Request.Cookies["AccessToken"];
            var refreshToken = context.HttpContext.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                if (await _authService.RefreshToken() != null)
                    return;

                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
        }
    }
}
