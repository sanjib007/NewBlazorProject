namespace Cr.UI.Data
{
    public class AuditableEntity
    {
        public long Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }
    }
}
