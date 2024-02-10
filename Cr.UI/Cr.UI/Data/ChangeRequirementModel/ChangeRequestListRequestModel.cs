namespace Cr.UI.Data.ChangeRequirementModel
{
    public class ChangeRequestListRequestModel
    {
        public string? Subject { get; set; }
        public string? RequestorName { get; set; }
        public string? UserId { get; set; }
        public string? Status { get; set; }
        public long? CrId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
