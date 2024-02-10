using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE
{
    public class radippool
    {
        [Key]
        public int id { get; set; }
        public int pool_name { get; set; }
        public string? framedipaddress { get; set; }
    }
}
