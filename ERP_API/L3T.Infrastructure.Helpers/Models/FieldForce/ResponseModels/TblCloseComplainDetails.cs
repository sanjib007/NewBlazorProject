using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class TblCloseComplainDetailsModel
    {
        [Key]
        public int AutoID { get; set; }
        public string? ComplainID { get; set; }
        public string? PendingStatus { get; set; }
        public DateTime? SolvedDatetime { get; set; }
        public string? TeamID { get; set; }
        public DateTime? TicketReceivedTime { get; set; }
    }
}
