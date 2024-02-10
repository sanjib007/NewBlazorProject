using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SelfCare.RequestModel
{
    public class ServiceCreateRequestModel
    {
        public string? ServiceName { get; set; }  // Bongo, HoiChoi, IP Phone
        public string? CustomerID { get; set; }
        public string? ServiceID { get; set; }
    }
}
