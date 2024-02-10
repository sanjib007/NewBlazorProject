using L3T.Infrastructure.Helpers.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class CrDefaultApprovalFlow : AuditableEntity
    {
        public string ApproverName { get; set; }
        public string ApproverDesignation { get; set; }
        public string ApproverDepartment { get; set; }
        public string Department { get; set; }
        public string ApproverEmpId { get; set; }
        public string ApproverRole { get; set; }
        public int ApproverFlow { get; set; }
        public int ParentId { get; set; }
        public bool IsPrincipleApprover { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
