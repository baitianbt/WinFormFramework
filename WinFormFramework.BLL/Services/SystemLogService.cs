using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;
using WinFormFramework.DAL;
using WinFormFramework.DAL.Entities;

namespace WinFormFramework.BLL.Services
{
    public class SystemLogService : ISystemLogService
    {
        private readonly IRepository<SystemLog> _logRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public SystemLogService(IRepository<SystemLog> logRepository, IMapper mapper, ILogService logger)
        {
            _logRepository = logRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SystemLogDTO> AddLogAsync(SystemLogDTO logDto)
        {
            try
            {
                var log = _mapper.Map<SystemLog>(logDto);
                log.LogTime = DateTime.Now;
                var result = await _logRepository.AddAsync(log);
                return _mapper.Map<SystemLogDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding system log", ex);
                throw;
            }
        }

        public async Task ClearLogsAsync(DateTime before)
        {
            try
            {
                var logs = await _logRepository.Query()
                    .Where(l => l.LogTime < before)
                    .ToListAsync();

                foreach (var log in logs)
                {
                    await _logRepository.DeleteAsync(log.Id);
                }

                _logger.Information($"Cleared logs before {before}");
            }
            catch (Exception ex)
            {
                _logger.Error("Error clearing logs", ex);
                throw;
            }
        }

        public async Task ExportLogsAsync(string filePath, DateTime startTime, DateTime endTime)
        {
            try
            {
                var logs = await GetLogsAsync(startTime, endTime);
                var csv = new StringBuilder();

                // 添加CSV头
                csv.AppendLine("时间,级别,消息,来源,用户名,IP地址,异常信息");

                // 添加数据行
                foreach (var log in logs)
                {
                    csv.AppendLine($"{log.LogTime:yyyy-MM-dd HH:mm:ss}," +
                                 $"{log.Level}," +
                                 $"\"{EscapeCsvField(log.Message)}\"," +
                                 $"\"{EscapeCsvField(log.Source)}\"," +
                                 $"\"{EscapeCsvField(log.UserName)}\"," +
                                 $"\"{EscapeCsvField(log.IpAddress)}\"," +
                                 $"\"{EscapeCsvField(log.Exception)}\"");
                }

                await File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8);
                _logger.Information($"Exported logs to {filePath}");
            }
            catch (Exception ex)
            {
                _logger.Error("Error exporting logs", ex);
                throw;
            }
        }

        public async Task<IEnumerable<SystemLogDTO>> GetLogsAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? level = null,
            string? keyword = null,
            int pageIndex = 1,
            int pageSize = 50)
        {
            try
            {
                var query = _logRepository.Query();

                if (startTime.HasValue)
                    query = query.Where(l => l.LogTime >= startTime.Value);

                if (endTime.HasValue)
                    query = query.Where(l => l.LogTime <= endTime.Value);

                if (!string.IsNullOrEmpty(level))
                    query = query.Where(l => l.Level == level);

                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(l => l.Message.Contains(keyword) ||
                                           l.Source.Contains(keyword) ||
                                           l.UserName.Contains(keyword));

                query = query.OrderByDescending(l => l.LogTime)
                           .Skip((pageIndex - 1) * pageSize)
                           .Take(pageSize);

                var logs = await query.ToListAsync();
                return _mapper.Map<IEnumerable<SystemLogDTO>>(logs);
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting logs", ex);
                throw;
            }
        }

        public async Task<int> GetLogsCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? level = null,
            string? keyword = null)
        {
            try
            {
                var query = _logRepository.Query();

                if (startTime.HasValue)
                    query = query.Where(l => l.LogTime >= startTime.Value);

                if (endTime.HasValue)
                    query = query.Where(l => l.LogTime <= endTime.Value);

                if (!string.IsNullOrEmpty(level))
                    query = query.Where(l => l.Level == level);

                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(l => l.Message.Contains(keyword) ||
                                           l.Source.Contains(keyword) ||
                                           l.UserName.Contains(keyword));

                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting logs count", ex);
                throw;
            }
        }

        private string EscapeCsvField(string? field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            return field.Replace("\"", "\"\"");
        }
    }
} 