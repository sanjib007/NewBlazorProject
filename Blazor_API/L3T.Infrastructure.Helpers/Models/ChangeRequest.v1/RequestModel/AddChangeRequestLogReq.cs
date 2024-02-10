using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AddChangeRequestLogReq
    {
        public long CrId { get; set; }
        public string CRChangeStatus { get; set; }
        public long? CrApprovalFlowId { get; set; }
        public long? AEId { get; set; }
        public int TaskFlow { get; set; }
        public string? Remark { get; set; }
        public string? Common1 { get; set; }
        public string? Common2 { get; set; }
    }
}
