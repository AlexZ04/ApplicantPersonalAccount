using Serilog;

namespace ApplicantPersonalAccount.Staff.Setup
{
    public class SetupLogs
    {
        public static void AddLogs(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration)
                => configuration.ReadFrom.Configuration(context.Configuration));
        }

        public static void ApplyLogs(WebApplication app)
        {
            app.UseSerilogRequestLogging();
        }
    }
}
