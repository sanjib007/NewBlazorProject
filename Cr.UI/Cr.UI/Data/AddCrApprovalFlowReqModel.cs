namespace Cr.UI.Data
{
    public class AddCrApprovalFlowReqModel
    {
        public long CrId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverDesignation { get; set; }
        public string ApproverDepartment { get; set; }
        public string ApproverEmpId { get; set; }
        public string ApproverFlow { get; set; }
        public string ParentId { get; set; }
        public string Status { get; set; }
        public string StatusDisplayName { get; set; }
        public string StatusDate { get; set; }
        public string? Remark { get; set; }
    }
}
