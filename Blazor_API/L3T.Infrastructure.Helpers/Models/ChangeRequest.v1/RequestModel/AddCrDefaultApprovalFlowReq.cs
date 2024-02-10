using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AddCrDefaultApprovalFlowReq
    {
        public string ApproverEmpId { get; set; }
        public int ParentId { get; set; }
        public string Department { get; set; }
        public bool IsPrincipleApprover { get; set;} = false;
	}
}
