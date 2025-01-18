namespace WinFormFramework.DAL.Entities
{
    public class Permission : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Group { get; set; } = string.Empty;
        public bool IsSystem { get; set; }

        // 导航属性
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
} 