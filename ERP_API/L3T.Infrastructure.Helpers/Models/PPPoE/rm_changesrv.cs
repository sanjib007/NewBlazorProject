using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.PPPoE
{
    public class rm_changesrv
    {
        [Key]
        public long id { get; set; }
        public string? username { get; set; }
        public long newsrvid { get; set; }
        public string? newsrvname { get; set; }
        public DateTime scheduledate { get; set; }
        public DateTime? requestdate { get; set; }
        public int status { get; set; }
        public string? requested { get; set; }
    }
}
