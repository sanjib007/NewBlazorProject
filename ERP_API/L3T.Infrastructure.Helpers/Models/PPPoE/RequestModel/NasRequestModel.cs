using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE.RequestModel
{
    public class NasRequestModel
    {

        public string? router_name { get; set; }  // Ankur Net
        public string? secret { get; set; }  //  secret
        public string? router_ip { get; set; } // Router IP
    }
}
