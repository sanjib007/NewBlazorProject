namespace IPV6ConfigBlazorWA.Model.DataModel
{
    public class BaseAuditModel
    {
        public long Id { get; set; }
        public int IsActive { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
