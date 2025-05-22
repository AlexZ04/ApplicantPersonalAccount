using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.Applicant.Setup
{
    public static class SetupAspNet
    {
        public static void AddAspNet(WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
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
