namespace WinFormFramework.BLL.DTOs
{
    public class SystemSettingDTO
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Group { get; set; } = "General";
        public bool IsSystem { get; set; }
    }
} 