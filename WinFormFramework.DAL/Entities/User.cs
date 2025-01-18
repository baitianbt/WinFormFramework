namespace WinFormFramework.DAL.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RealName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime? LastLoginTime { get; set; }
        
        // 导航属性
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
} 