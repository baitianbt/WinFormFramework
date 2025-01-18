namespace WinFormFramework.DAL.Entities
{
    public class SystemSetting : BaseEntity
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Group { get; set; } = "General";
        public bool IsSystem { get; set; }
    }
} 