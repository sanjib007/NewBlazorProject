namespace IPV6ConfigSetup.DataAccess.Model
{
    public class IPV6SetupRequestModel
    {
        public string ParentSubnet { get; set; }
        public long IPV6_BTSSubnetId { get; set; }
        public int? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CustomerType { get; set; }
        public long? DistributorId { get; set; }
        public string? DistributorName { get; set; }
        public long? PackageId { get; set; }
        public string? PackageName { get; set; }
        public long? BTSId { get; set; }
        public string? BTSName { get; set; }
        public long? RouterNameId { get; set; }
        public string? RouterName { get; set; }
        public string? RouterHostName { get; set; }
        public string? HostName { get; set; }
        public string? RouterSwitchIP { get; set; }
        public string? Noofporf { get; set; }
        public string? PoolId { get; set; }
        public string? PoolName { get; set; }
        public string? VLAN { get; set; }
        public string? Gateway { get; set; }
        public string? Remarkes { get; set; }
        
        
       
        public List<IPV6_CustomerSubnet64Model> IPV6_CustomerSubnets { get; set; }
    }
}
