namespace IPV6ConfigSetup.DataAccess.Model
{
    public class BaseAuditModel
    {
        public long? Id { get; set; }
        public int? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
