using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE.RequestModel
{
    public class IpPoolRequestModel
    {
        public string pool_name { get; set; }
        public string? first_ip { get; set; }
        public string? last_ip { get; set; }

    }
}
