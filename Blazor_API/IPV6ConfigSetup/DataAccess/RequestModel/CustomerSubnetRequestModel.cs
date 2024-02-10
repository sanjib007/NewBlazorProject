namespace IPV6ConfigSetup.DataAccess.RequestModel
{
    public class CustomerSubnetRequestModel
    {
        public required string CustomerSubnet { get; set; }
        public long IPV6_ParentSubnetId { get; set; }
    }
}
