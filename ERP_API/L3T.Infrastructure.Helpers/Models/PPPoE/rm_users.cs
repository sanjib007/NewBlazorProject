using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE
{
    public class rm_users
    {
        [Key]
        public string? username { get; set; }
        public string? password { get; set; }
        public DateTime expiration { get; set; }
        public int srvid { get; set; }
        public string? staticipcpe { get; set; }
        public int ipmodecpe { get; set; }
        public string? createdby { get; set; }
        public string? owner { get; set; }
        public DateTime createdon { get; set; }
    }
}
