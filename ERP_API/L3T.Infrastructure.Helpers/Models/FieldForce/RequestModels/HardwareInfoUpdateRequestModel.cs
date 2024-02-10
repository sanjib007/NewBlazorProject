using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class HardwareInfoUpdateRequestModel
    {
        public string TicketId { get; set; }       
        public string TeamName { get; set; }
        public string Comments { get; set; }      
    }
}
