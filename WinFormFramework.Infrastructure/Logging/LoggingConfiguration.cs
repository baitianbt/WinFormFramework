using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace WinFormFramework.Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        public static IServiceCollection AddLogging(this IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File("logs/app-.log",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            services.AddSingleton<ILogger>(logger);
            services.AddScoped<ILogService, SerilogService>();

            return services;
        }
    }
} 