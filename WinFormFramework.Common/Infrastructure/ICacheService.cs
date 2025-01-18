namespace WinFormFramework.Common.Infrastructure
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        void Remove(string key);
        bool Contains(string key);
        void Clear();
    }
} 