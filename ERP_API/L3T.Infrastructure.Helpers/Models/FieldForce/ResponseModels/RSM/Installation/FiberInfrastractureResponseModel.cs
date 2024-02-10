using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class FiberInfrastractureResponseModel
    {
        public string? BtsName { get; set; }
        public string? BRAS { get; set; }
        public string? BrasIp { get; set; }
        public string? SplitterLocation { get; set; }
        public string? OltBrand { get; set; }
        public string? OltName { get; set; }
        public string? OltIp { get; set; }
        public string? Pon { get; set; }
        public string? Port { get; set; }
        public string? Laser { get; set; }
        public string? Ip { get; set; }
        public string? Gateway { get; set; }
        public string? SubnetMask { get; set; }
        public string? IpV6 { get; set; }
        public string? Vlan { get; set; }
        public string? Remarks { get; set; }
    }
}
