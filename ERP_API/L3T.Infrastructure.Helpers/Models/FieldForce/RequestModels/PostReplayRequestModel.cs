using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class PostReplayRequestModel
    {
        public string ticketNo { get; set; }
        public string? supporTypeName { get; set; }
        public bool chksupportType { get; set; }
        public string postText { get; set; }
        public string? followUpName { get; set; }
        public bool isFollowUpVisible { get; set; }
        public string? pendingReasonName { get; set; }
        public int? pendingReasonId { get; set; }
        public int? followUpId { get; set; }
        public bool chkIsUrgent { get; set; }
        public string? tokenID { get; set; }
        public string? vicidialId { get; set; }
        public DateTime sheduleDate { get; set; }
    }
}
