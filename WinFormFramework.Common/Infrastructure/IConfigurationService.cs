using Microsoft.Extensions.Configuration;

namespace WinFormFramework.Common.Infrastructure
{
    public interface IConfigurationService
    {
        T? GetValue<T>(string key);
        T GetValue<T>(string key, T defaultValue);
        string? GetConnectionString(string name);
        IConfiguration GetConfiguration();
    }
} 