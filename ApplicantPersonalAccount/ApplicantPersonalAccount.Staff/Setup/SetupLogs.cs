using Serilog;

namespace ApplicantPersonalAccount.Staff.Setup
{
    public class SetupLogs
    {
        public static void AddLogs(WebApplicationBuilder builder)
        {
            var logDirectory = Path.Combine("../../Logs", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            Directory.CreateDirectory(logDirectory);

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        Path.Combine(logDirectory, "admin_panel-log-.txt"),
                        rollingInterval: RollingInterval.Day);
            });
        }

        public static void ApplyLogs(WebApplication app)
        {
            app.UseSerilogRequestLogging();
        }
    }
}
