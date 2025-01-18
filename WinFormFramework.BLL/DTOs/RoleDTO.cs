namespace WinFormFramework.BLL.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSystem { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
} 