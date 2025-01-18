namespace WinFormFramework.BLL.Interfaces
{
    public interface ISystemLogService
    {
        Task<IEnumerable<SystemLogDTO>> GetLogsAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? level = null,
            string? keyword = null,
            int pageIndex = 1,
            int pageSize = 50);
        
        Task<int> GetLogsCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? level = null,
            string? keyword = null);

        Task<SystemLogDTO> AddLogAsync(SystemLogDTO logDto);
        Task ClearLogsAsync(DateTime before);
        Task ExportLogsAsync(string filePath, DateTime startTime, DateTime endTime);
    }
} 