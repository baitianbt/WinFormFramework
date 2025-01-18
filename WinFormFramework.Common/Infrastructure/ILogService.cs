namespace WinFormFramework.Common.Infrastructure
{
    public interface ILogService
    {
        void Debug(string message, params object[] args);
        void Information(string message, params object[] args);
        void Warning(string message, params object[] args);
        void Error(string message, Exception? exception = null);
        void Error(string message, params object[] args);
    }
} 