using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class ForwardTicketRequestModel
    {
        public string TicketId { get; set; }
        public string TicketTitle { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public string Category { get; set; }
        public string txtteamid { get; set; }
        public string ForwardToText { get; set; }
        public string ForwardToValue { get; set; }
        public string? SupportType { get; set; }
        public DateTime ForwardTime { get; set; }
        public string? InformingPerson { get; set;}
        public string Comments { get; set; }
        public bool EmailToTicketRealatedEmployee { get; set; }
        public bool SolvedAndForward { get; set; }

    }
}
