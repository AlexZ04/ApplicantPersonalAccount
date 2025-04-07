using ApplicantPersonalAccount.API.ExceptionHandler;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.API.Setup
{
    public static class SetupAspNet
    {
        public static void AddAspNet(WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public static void UseAspNet(WebApplication app)
        {
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.MapControllers();
        }
    }
}
