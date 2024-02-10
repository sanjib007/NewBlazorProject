namespace Cr.UI.Data.ApprovalFlow
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
