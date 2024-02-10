using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class IpTelephonyUpdateRequestModel
    {
        public string TicketId { get; set; }
        public string CliCode { get; set; }
        public string SlNo { get; set; }
        public string TeamName { get; set; }
        public string PhoneNo { get; set; }
        public string NotForIP { get; set; }
        public string Comments { get; set; }
        public string ISP { get; set; }
        public DateTime CommencementDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CpeText { get; set; }
    }
}
