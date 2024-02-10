using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class tbl_Team_InfoResponseModel
    {
        public string? Team_id { get; set; }
        public string? Team_Name { get; set; }
        public string? Team_Desc { get; set; }
        public string? Team_Email { get; set; }
        public string? Team_IPost { get; set; }
        public string? Team_taskclose { get; set; }
        public string? Team_TOpen { get; set; }
        public string? Team_TForward { get; set; }
        public string? Team_InsSolve { get; set; }
        public string? Team_ViewAll { get; set; }
        public int? Tstatus { get; set; }
        //public int? TSerial { get; set; }
        //public int? GMSerial { get; set; }
        //public int? TeamStatusDistributor { get; set; }
        //public int? TeamSurveyStatus { get; set; }
        public int? Status { get; set; }
    }
}
