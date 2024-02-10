using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class ChangeRequestListReqModel
    {
        public string? Subject { get; set; }
        public string? RequestorName { get; set; }
        public string? UserId { get; set; }
        public string? ApproverEmpId { get; set; }
        public string? Status { get; set; }
        public string? ApproverStatus { get; set; }
        public long? CrId { get; set; }
        public string? Department { get;set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
