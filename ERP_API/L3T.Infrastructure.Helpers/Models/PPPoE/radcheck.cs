using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE
{
    public class radcheck
    {
        [Key]
        public long id { get; set; }
        public string? username { get; set; }
        public string? attribute { get; set; }
        public string? op { get; set; }
        public string? value { get; set; }
    }
}
