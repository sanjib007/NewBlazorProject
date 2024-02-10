using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class InstallationSheduleUpdateRequestModel
    {
        public string TicketId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string Remarks { get; set; }
    }
}
