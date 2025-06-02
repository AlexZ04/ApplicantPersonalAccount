using ApplicantPersonalAccount.Common.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApplicantPersonalAccount.Staff.Setup
{
    public class SetupAuth
    {
        public static void AddAuth(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["AccessToken"];
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.Configure<CookieOptions>(options =>
            {
                options.HttpOnly = true;
                options.SameSite = SameSiteMode.Strict;
                options.Secure = true;
            });

            builder.Services.AddAuthorization();
        }

        public static void UseAuth(WebApplication app)
        {
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
