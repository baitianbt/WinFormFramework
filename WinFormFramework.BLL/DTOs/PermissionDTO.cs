namespace WinFormFramework.BLL.DTOs
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Group { get; set; } = string.Empty;
        public bool IsSystem { get; set; }
        public bool IsGranted { get; set; }
    }
} 