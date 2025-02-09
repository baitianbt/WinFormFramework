namespace WinFormFramework.DAL.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        // 导航属性
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
} 