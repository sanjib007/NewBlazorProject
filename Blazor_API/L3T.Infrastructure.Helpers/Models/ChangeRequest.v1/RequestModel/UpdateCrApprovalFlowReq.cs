using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class UpdateCrApprovalFlowReq
    {
        public long Id { get; set; }
        public long CrId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverDesignation { get; set; }
        public string ApproverDepartment { get; set; }
        public string ApproverEmpId { get; set; }
        public int ApproverFlow { get; set; }
        public int ParentId { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string? Remark { get; set; }


    }
}
