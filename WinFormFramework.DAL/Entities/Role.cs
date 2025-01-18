namespace WinFormFramework.DAL.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSystem { get; set; }

        // 导航属性
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
} 