namespace IPV6ConfigSetup.DataAccess.Model
{
    public class IPV6_CustomerSubnet64Model : BaseAuditModel
    {
        public string CustomerSubnet { get; set; }
        public long IPV6_ParentSubnetId { get; set; }
        public int? IsUsed { get; set; } = 0;
        public string? SubscriberID { get; set;}
    }
}
