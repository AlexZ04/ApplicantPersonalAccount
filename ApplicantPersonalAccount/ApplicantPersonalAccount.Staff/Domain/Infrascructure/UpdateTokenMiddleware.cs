using Microsoft.Extensions.DependencyInjection;
using ApplicantPersonalAccount.Staff.Domain.Services.Interfaces;

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

                await authService.RefreshToken();
            }

            await _next(context);
        }
    }
}
