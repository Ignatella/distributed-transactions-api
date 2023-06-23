using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace DistributedTransactionsApi.Core;

internal static class ApplicationLogger
{
    public static ConfigureHostBuilder UseLogging(this ConfigureHostBuilder host, ConfigurationManager configuration)
    {
        const string logOutputTemplate =
            @"[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

        const string logDir = "logs";
        Directory.CreateDirectory(logDir);
        const string logFile = $"{logDir}/log.txt";

        if (configuration.GetValue<string>("BankRichLogs")?.ToLower() == "true")
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 2)
                .WriteTo.Console(outputTemplate: logOutputTemplate, theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .Enrich.FromLogContext()
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3)
                .WriteTo.Console(outputTemplate: logOutputTemplate, theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }

        host.UseSerilog();

        return host;
    }
}