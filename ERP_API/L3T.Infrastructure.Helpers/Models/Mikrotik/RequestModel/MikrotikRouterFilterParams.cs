using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel
{
    public class MikrotikRouterFilterParams : MikrotikRouterCommonModel
    {
        public string? Package { get; set; } // list
        public string? CustomerIP { get; set; } // address
        public string? CustomerId { get; set; } // comment
        public string? Disable { get; set; } // disable
        public string CallerId { get; set; }
        public string? UniqueId { get; set; }
        public string? UserId { get; set; }

    }
}
