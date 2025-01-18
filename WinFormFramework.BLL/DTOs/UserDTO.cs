namespace WinFormFramework.BLL.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string RealName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
} 