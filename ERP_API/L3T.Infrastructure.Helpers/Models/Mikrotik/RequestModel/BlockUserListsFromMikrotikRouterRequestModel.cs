using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel
{
    public class BlockUserListsFromMikrotikRouterRequestModel : MikrotikRouterCommonModel
    {
        public string CustomerIp { get; set; }
        public string? CallerId { get; set; }
        public string Package { get; set; }
        public string SubId { get; set; }
    }
}
