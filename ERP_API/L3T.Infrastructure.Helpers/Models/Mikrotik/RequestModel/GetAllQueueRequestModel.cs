using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel
{
    public class GetAllQueueRequestModel : MikrotikRouterCommonModel
    {
        public string? Name { get; set; }
        public string? Target {  get; set; }
        public string CallerId { get; set;}
    }
}
