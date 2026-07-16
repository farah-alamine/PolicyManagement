namespace PolicyManagement.Core.Entities.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public Guid RecordGuid { get; set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime? LastUpdatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }

    }
}
