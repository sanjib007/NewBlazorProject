using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.TicketEntity
{
    public class TicketEntry
    {
        [Key]
        public long TicketId { get; set; }
        public string brCliCode { get; set; }
        public int brSlNo { get; set; }
        public string MqID { get; set; }
        public DateTime ReceiveDateTime { get; set; } = DateTime.Now;
        public DateTime FaultOccured { get; set; }
        public int ComplainCategory { get; set; }
        public string Complains { get; set; }
        public string ComplainSource { get; set; }
        public string LedStatus { get; set; }
        [Required]
        public string PendingTeamID { get; set; }
        [Required]
        public string ComplainReceiveby { get; set; }
        public string Comments { get; set; }
        public string TaskStatus { get; set; }
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;
        public string LastComments { get; set; }
        public string CompleteBy { get; set; }
        public DateTime CompleteDate { get; set; }
        public string RefeNoActivity { get; set; }
        [Required]
        public DateTime ForwardDate { get; set; }
        [Required]
        public string AssignEngineer { get; set; } = "No";
        public string SupportType { get; set; }
        public DateTime ExecuteDate { get; set; }
        public int CloseCategory { get; set; }
        [Required]
        public int DaysExtension { get; set; }
        [Required]
        public int CauseOfDelay { get; set; }
        public int CauseOfTermination { get; set; }
        public DateTime TerminateExcuteDate { get; set; }
        public string DeviceCollectionStatus { get; set; }
        public string GenerateTicketSupportType { get; set; }
        public int PendingReasonID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string vicidial_call_id { get; set; }
        public int TraceLess_Schedule { get; set; }
        public int ScheduleStatus { get; set; }
        public string ONUSTATUS { get; set; }
        public string ONULASER { get; set; }
        public string ONUBW { get; set; }
        public string ONUPORT { get; set; }
        public string USERMAC { get; set; }
        public string VLAN { get; set; }
        public string PON { get; set; }
        public string REBOOT { get; set; }
        public string SourceMobileNo { get; set; }
        public bool IsBussinessHour { get; set; }
        public string IsUrgentSupport { get; set; }
        public string SourceOfInformation { get; set; }
        public int TaskCategory { get; set; }
        public int TaskNature { get; set; }
    }
}
