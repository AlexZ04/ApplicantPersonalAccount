using Microsoft.Extensions.DependencyInjection;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;
using ApplicantPersonalAccount.Common.Constants;

namespace ApplicantPersonalAccount.Staff.Domain.Infrascructure
{
    public class UpdateTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public UpdateTokenMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var accessToken = context.Request.Cookies["AccessToken"];
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                using var scope = _serviceProvider.CreateScope();
                var authService = scope.ServiceProvider.GetRequiredService<IStaffAuthService>();

                var tokens = await authService.RefreshToken();

                if (tokens != null)
                {
                    context.Response.Cookies.Append("AccessToken", tokens.AccessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = tokens.AccessExpireTime
                    });

                    context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddDays(GeneralSettings.REFRESH_TOKEN_LIFETIME)
                    });

                    context.Request.Headers["Authorization"] = $"Bearer {tokens.AccessToken}";
                }
            }

            await _next(context);
        }
    }
}
