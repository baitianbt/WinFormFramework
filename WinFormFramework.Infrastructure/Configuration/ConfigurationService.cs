using Microsoft.Extensions.Configuration;
using WinFormFramework.Common.Infrastructure;

namespace WinFormFramework.Infrastructure.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T? GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            return _configuration.GetValue<T>(key, defaultValue);
        }

        public string? GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }

        public IConfiguration GetConfiguration()
        {
            return _configuration;
        }
    }
} 