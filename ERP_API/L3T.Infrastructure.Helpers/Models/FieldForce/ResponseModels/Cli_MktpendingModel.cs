using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class Cli_MktpendingModel
    {
        [Key]
        public string Refno { get; set; }
        public string Mkt_group { get; set; }
        public string Pending_for { get; set; }
        public string PStatus { get; set; }
        public string Pending_for_team { get; set; }
        public int PendingReason { get; set; }
        public string LastComments { get; set; }
        public DateTime LastCommentsDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? Sceduleby { get; set; }
        public int ID { get; set; }

    }
}
