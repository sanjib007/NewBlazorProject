using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM
{
    
    public class RSM_Complain_Details
    {
        [Key]
        public int RefNo { get; set; }
        public string? CustomerID { get; set; }
        public DateTime? ReceiveDateTime { get; set; }
        public DateTime? FaultOccured { get; set; }
        public DateTime? ActualReceiveDatetime { get; set; }
        public string? ComplainCategory { get; set; }
        public string? Complains { get; set; }
        public string? ComplainSource { get; set; }
        public string? LedStatus { get; set; }
        public string? PendingTeamID { get; set; }
        public string? ComplainReceiveby { get; set; }
        public string? Comments { get; set; }
        public string? TaskStatus { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string? LastComments { get; set; }
        public string? Completeby { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? RefeNoActivity { get; set; }
        public DateTime? ForwardDate { get; set; }
        public string? AssignEngineer { get; set; }
        public string? SupportType { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public int? CloseCategory { get; set; }
        public int? DaysExtension { get; set; }
        public int? CauseOfDelay { get; set; }
        public int? CauseOfTermination { get; set; }
        public DateTime? TerminateexcuteDate { get; set; }
        public string? DeviceCollectionStatus { get; set; }
        public string? GenerateTicketSupportType { get; set; }
        public int? PendingReasonID { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? vicidial_call_id { get; set; }
        public int? TraceLess_Schedule { get; set; }
        public int? ScheduleStatus { get; set; }
        public string? ONUSTATUS { get; set; }
        public string? ONULASER { get; set; }
        public string? ONUBW { get; set; }
        public string? ONUPORT { get; set; }
        public string? USERMAC { get; set; }
        public string? VLAN { get; set; }
        public string? PON { get; set; }
        public string? REBOOT { get; set; }
        public string? SourceMobileNo { get; set; }
        public string? IsBussinessHour { get; set; }
        public string? IsUrgentSupport { get; set; }
    }
}
