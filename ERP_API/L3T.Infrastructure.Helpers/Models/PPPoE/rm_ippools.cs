using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE
{
    public class rm_ippools
    {
        [Key]
        public int id { get; set; }
        public int type { get; set; }
        public string? name { get; set; }  // Ankur Net 
        public string? fromip { get; set; }  // 192.168.1.2 
        public string? toip { get; set; }  // 192.168.1.254
    }
}
