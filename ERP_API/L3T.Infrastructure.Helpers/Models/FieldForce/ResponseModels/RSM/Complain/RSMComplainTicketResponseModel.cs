using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class RSMComplainTicketResponseModel
    {
        [Key]
        public int RefNo { get; set; }
        public string? CustomerID { get; set; }
        public string? HeadOfficeName { get; set; }
        public DateTime? ReceiveDateTime { get; set; }
        public DateTime? ForwardDate { get; set; }
        public string? Date { get; set; }
        public string? TicketTypeName { get; set; }
        public string? brAddress1 { get; set; }
        public string? phone_no { get; set; }
        public string? Comments { get; set; }
        public string? NatureName { get; set; }
        public string? brSupportOffice { get; set; }
        public string? Team_Name { get; set; }
        public string? Area { get; set; }
        public string? user_name { get; set; }
        public string? Complains { get; set; }
        public string? LastComments { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public string? AssignEngineer { get; set; }
        public string? AssignEng { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public int? ScheduleStatus { get; set; }
        public string? ComplainSource { get; set; }
        public string? TaskPendingStatus { get; set; }
        public string? SupportType { get; set; }
    }
}
