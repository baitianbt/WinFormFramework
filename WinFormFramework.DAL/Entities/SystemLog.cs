namespace WinFormFramework.DAL.Entities
{
    public class SystemLog : BaseEntity
    {
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public string? Source { get; set; }
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public DateTime LogTime { get; set; }
    }
} 