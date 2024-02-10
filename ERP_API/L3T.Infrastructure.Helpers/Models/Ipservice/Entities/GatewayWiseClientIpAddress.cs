using L3T.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Ipservice.Entities
{
    public class GatewayWiseClientIpAddress : AuditableEntity
    {
        public long GatewayIpAddressId { get; set; }
        public virtual GatewayIpAddress GatewayIpAddresses { get; set; }
        public long PackageId { get; set;}
        public string IpAddress { get; set;}
        public string PoolName { get; set; }
        public string SubNetMask { get; set; }
        public string LookBackAddress { get; set; }
        public string SubscriberId { get; set;}
        public int SubscriberSlNo { get; set;}
        public string UsedStatus { get; set;}
        public string Remarks { get; set;}
        public Boolean Status { get; set; } = true;
        

    }
}
