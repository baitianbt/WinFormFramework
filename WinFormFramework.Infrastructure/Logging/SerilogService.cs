using Serilog;
using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Infrastructure;

namespace WinFormFramework.Infrastructure.Logging
{
    public class SerilogService : ILogService
    {
        private readonly ILogger _logger;
        private readonly ISystemLogService? _systemLogService;
        private readonly string? _currentUser;

        public SerilogService(
            ILogger logger,
            ISystemLogService? systemLogService = null,
            string? currentUser = null)
        {
            _logger = logger;
            _systemLogService = systemLogService;
            _currentUser = currentUser;
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
            LogToSystemAsync("Debug", string.Format(message, args)).ConfigureAwait(false);
        }

        public void Information(string message, params object[] args)
        {
            _logger.Information(message, args);
            LogToSystemAsync("Information", string.Format(message, args)).ConfigureAwait(false);
        }

        public void Warning(string message, params object[] args)
        {
            _logger.Warning(message, args);
            LogToSystemAsync("Warning", string.Format(message, args)).ConfigureAwait(false);
        }

        public void Error(string message, Exception? exception = null)
        {
            if (exception != null)
                _logger.Error(exception, message);
            else
                _logger.Error(message);

            LogToSystemAsync("Error", message, exception).ConfigureAwait(false);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
            LogToSystemAsync("Error", string.Format(message, args)).ConfigureAwait(false);
        }

        private async Task LogToSystemAsync(string level, string message, Exception? exception = null)
        {
            if (_systemLogService == null)
                return;

            try
            {
                var logDto = new SystemLogDTO
                {
                    Level = level,
                    Message = message,
                    Exception = exception?.ToString(),
                    Source = exception?.Source,
                    UserName = _currentUser,
                    IpAddress = GetClientIp(),
                    LogTime = DateTime.Now
                };

                await _systemLogService.AddLogAsync(logDto);
            }
            catch
            {
                // 忽略系统日志记录失败
            }
        }

        private string? GetClientIp()
        {
            try
            {
                return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                    .ToString();
            }
            catch
            {
                return null;
            }
        }
    }
} 