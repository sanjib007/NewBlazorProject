using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class SendSmsRequestModel
    {
        public string TicketRefNo { get; set; }
        public string? CustomerPhoneNo { get; set; }
        public string? ContactPhoneNo { get; set; }
        public string? additionalEmail { get; set; }
    }
}
