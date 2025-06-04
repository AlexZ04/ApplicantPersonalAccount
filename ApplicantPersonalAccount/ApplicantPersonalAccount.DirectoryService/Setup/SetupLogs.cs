using Serilog;

namespace ApplicantPersonalAccount.DirectoryService.Setup
{
    public class SetupLogs
    {
        public static void AddLogs(WebApplicationBuilder builder)
        {
            var logDirectory = Path.Combine("../../Logs", DateTime.Now.ToString("yyyy-MM-dd"));
            Directory.CreateDirectory(logDirectory);

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        Path.Combine(logDirectory, "directory-log-.txt"),
                        rollingInterval: RollingInterval.Day);
            });
        }

        public static void ApplyLogs(WebApplication app)
        {
            app.UseSerilogRequestLogging();
        }
    }
}
