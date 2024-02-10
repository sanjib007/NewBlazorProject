using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class Kh_IpAddressResponseModel
    {
        [Key]
        public int ID { get; set; }
        public int BTSID { get; set; }
        public string RouterName { get; set; }
        public string RouterPort { get; set; }
        public string PoolName { get; set; }
        public string VLAN { get; set; }
        public string IPAddress { get; set; }
        public string Gateway { get; set; }
        public string SubnetMask { get; set; }
        public string RouterHostName { get; set; }
        public string LoopBackAddress { get; set; }
        public string RouterNameModel { get; set; }
        public string SubscriberID { get; set; }
        public int SubscriberSlNo { get; set; }
        public string UsedStatus { get; set; }
        public string Remarks { get; set; }
        public string HostName { get; set; }
        public string RouterSwitchIP { get; set; }
    }
}
