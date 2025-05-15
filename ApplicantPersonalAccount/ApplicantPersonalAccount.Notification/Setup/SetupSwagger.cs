using Microsoft.OpenApi.Models;

namespace ApplicantPersonalAccount.Notification.Setup
{
    public class SetupSwagger
    {
        public static void AddSwagger(WebApplicationBuilder builder)
        {

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static void UseSwagger(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
