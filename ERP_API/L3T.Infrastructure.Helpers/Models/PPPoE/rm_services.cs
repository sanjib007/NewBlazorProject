using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE
{
    public class rm_services
    {
        [Key]
        public long srvid { get; set; }
        public long enableservice { get; set; }
        public string? srvname { get; set; }
    }
}
