using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.OLT
{
    public class OltBwInfo
    {
        [Key]
        public int ID { get; set; }
        public string OltIp { get; set; }
        public string OltName { get; set; }
        public int OltBrand { get; set; }

    }
}
