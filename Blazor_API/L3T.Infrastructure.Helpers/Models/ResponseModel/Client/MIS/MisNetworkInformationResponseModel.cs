using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
    [Keyless]
    public class MisNetworkInformationResponseModel
    {
        public string? CustomerID { get; set; }
        public string? AssignedPackage { get; set; }
        public string? BrasIP { get; set; }
        public string? IPv4 { get; set; }
        public string? Subnet { get; set; }
        public string? IPv6 { get; set; }
        public string? VLAN { get; set; }
        public string? OLTPON { get; set; }
        public string? OLT { get; set; }
        public string? BTS { get; set; }
        public string? MACAddress { get; set; }       
        public string? CustomerCategory { get; set; }
    }
}
