using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class RsmInstallationAddCommentsRequestModel
    {
        public string TicketNo { get; set; }
        public string SubscriberCode { get; set; }
        public int? PendingReasonId { get; set; }
        public string? Comments { get; set; }
    }
}
