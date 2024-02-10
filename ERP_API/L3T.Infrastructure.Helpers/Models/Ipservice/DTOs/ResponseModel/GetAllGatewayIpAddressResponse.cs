using L3T.Infrastructure.Helpers.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.ResponseModel
{
    public class GetAllGatewayIpAddressResponse 
    {
        public long Id { get; set; }
        public long BtsId { get; set; }
        public long DistributorId { get; set; }
        public string RouterType { get; set; }
        public string RouterName { get; set; }
        public string RouterPort { get; set; }
        public string GateWay { get; set; }
        public string RouterHostName { get; set; }
        public string RouterModelName { get; set; }
        public string RouterSwitchIp { get; set; }
        public string Vlan { get; set; }
        public string HostName { get; set; }
        public string Remarks { get; set; }
        public Boolean Status { get; set; } = true;
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
