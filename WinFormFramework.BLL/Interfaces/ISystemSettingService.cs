namespace WinFormFramework.BLL.Interfaces
{
    public interface ISystemSettingService
    {
        Task<SystemSettingDTO?> GetByKeyAsync(string key);
        Task<IEnumerable<SystemSettingDTO>> GetByGroupAsync(string group);
        Task<IEnumerable<SystemSettingDTO>> GetAllAsync();
        Task<SystemSettingDTO> CreateAsync(SystemSettingDTO settingDto);
        Task UpdateAsync(SystemSettingDTO settingDto);
        Task DeleteAsync(int id);
        Task<string> GetSettingValueAsync(string key, string defaultValue = "");
        Task SaveSettingValueAsync(string key, string value);
    }
} 