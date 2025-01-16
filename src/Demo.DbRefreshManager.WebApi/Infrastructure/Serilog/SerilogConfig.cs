using Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;
using Serilog;
using Serilog.Debugging;
using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Serilog;

/// <summary>
/// Serilog configuration.
/// </summary>
public static class SerilogConfig
{
    /// <summary>
    /// Get configured serilog logger.
    /// </summary>
    public static ILogger GetConfiguredLogger(IWebHostEnvironment environment, IConfiguration configuration)
    {
        // Enable serilog debugging exceptions.
        if (environment.IsDevelopment())
        {
            SelfLog.Enable(msg => Debug.WriteLine(msg));
        }

        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration);

        // Шаблон для вывода в консоль для development.
        string devTemplate = string.Join("",
            "[{Level:u3}][{Timestamp:o}]{NewLine}",
            "[{SourceContext}]{NewLine}",
            "{Message:lj}{NewLine}",
            "{Exception}{NewLine}");

        // Логирование для локальной разработки.
        if (environment.IsDevelopment())
        {
            loggerConfig.WriteTo.Async(w => w.Console(outputTemplate: devTemplate));

            if (!environment.IsInContainer())
            {
                loggerConfig.WriteTo.Async(w => w.Debug(outputTemplate: devTemplate));
            }
        }
        // Production logs.
        else
        {
            loggerConfig.WriteTo.Async(w => w.Console(outputTemplate: devTemplate));
        }

        return loggerConfig.CreateLogger();
    }
}
