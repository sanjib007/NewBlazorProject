namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class CrListFilterReqModel
    {
        
        public string? DepartmentName { get; set; }
        public string? RequestorName { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? UserId { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
       
       


    }
}
