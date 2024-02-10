using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AddCrApprovalFlowReq
    {
        public long CrId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverDesignation { get; set; }
        public string ApproverDepartment { get; set; }
        public string ApproverEmpId { get; set; }
        public int ApproverFlow { get; set; }
        public int ParentId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string? Remark { get; set; }


    }
}
