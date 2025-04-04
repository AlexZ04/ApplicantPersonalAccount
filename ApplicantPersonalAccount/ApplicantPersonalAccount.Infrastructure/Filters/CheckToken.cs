using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ApplicantPersonalAccount.Infrastructure.Filters
{
    public class CheckToken : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = await context.HttpContext.GetTokenAsync("access_token");

            var tokenCache = context.HttpContext.RequestServices.GetService<IDistributedCache>();

            if (token == null || tokenCache == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            string? isTokenCached = await tokenCache.GetStringAsync($"blacklist:access:{token}");

            if (isTokenCached != null)
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
