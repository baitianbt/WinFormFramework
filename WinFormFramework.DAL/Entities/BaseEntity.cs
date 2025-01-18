namespace WinFormFramework.DAL.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? UpdateBy { get; set; }
        public bool IsDeleted { get; set; }
    }
} 