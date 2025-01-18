using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WinFormFramework.Infrastructure.Caching;
using WinFormFramework.Infrastructure.Configuration;
using WinFormFramework.Infrastructure.FileStorage;
using WinFormFramework.Infrastructure.Logging;

namespace WinFormFramework.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            string fileStoragePath)
        {
            // 添加配置服务
            services.AddSingleton<IConfigurationService>(new ConfigurationService(configuration));

            // 添加日志服务
            services.AddLogging();

            // 添加缓存服务
            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();

            // 添加文件存储服务
            services.AddSingleton<IFileStorageService>(new LocalFileStorageService(fileStoragePath));

            return services;
        }

        public static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
} 