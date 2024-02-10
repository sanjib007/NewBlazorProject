namespace Cr.UI.Data
{
    public class AssignEmployeeListResponse
    {
        public long? Id { get; set; }
        public string? EmpId { get; set; }
        public string? EmpName { get; set; }
        public long CrId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalDay { get; set; }
        public string? Task { get; set; }
        public bool? DeleteStatus { get; set; } = false;
        public int Total { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
