using L3T.Infrastructure.Helpers.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class CrApprovalFlow : AuditableEntity
    {
        public long CrId { get; set; }
        public string? ApproverName { get; set; }
        public string? ApproverEmail { get; set; }
        public string? ApproverDesignation { get; set; }
        public string? ApproverDepartment { get; set; }
        public string? Department { get; set; }
        public string? ApproverEmpId { get; set; }
        public string? ApproverRole { get; set; }
        public int ApproverFlow { get; set; }
        public long CrDefaultApproverFlowId { get; set; }
        public int ParentId { get; set; }        
        public string? Status { get; set; }
        public string? StatusDisplayName { get; set; }
        public DateTime? StatusDate { get; set; }
        public string? Remark { get; set; }
		public bool IsPrincipleApprover { get; set; } = false;
		public bool IsActive { get; set; } = true;
	}
}
