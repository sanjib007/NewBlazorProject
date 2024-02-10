namespace Cr.UI.Data
{
    public class AssignEmployeeListReqModel
    {
        public string? CrId { get; set; }
        public string? EmpId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;
    }
}
