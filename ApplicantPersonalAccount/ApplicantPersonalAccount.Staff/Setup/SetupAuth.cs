using ApplicantPersonalAccount.Common.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ApplicantPersonalAccount.Staff.Setup
{
    public class SetupAuth
    {
        public static void AddAuth(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Account/Login" +
                                (context.Request.Path.HasValue ? $"?returnUrl={context.Request.Path}" : ""));

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.Configure<CookieOptions>(options =>
            {
                options.HttpOnly = true;
                options.SameSite = SameSiteMode.None;
                options.Secure = true;
            });

            builder.Services.AddAuthorization();
        }

        public static void UseAuth(WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
