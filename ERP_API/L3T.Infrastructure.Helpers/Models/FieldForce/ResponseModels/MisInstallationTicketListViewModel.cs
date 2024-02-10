using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class MisInstallationTicketListViewModel
    {
        public string Mkt_group { get; set; }
        public string Pending_for { get; set; }
        public string CompanyName { get; set; }
        public string TrackingInfo { get; set; }
        public string SalesPerson { get; set; }
        public DateTime EntryDate { get; set; }
        public string PStatus { get; set; }
        public DateTime updatetime { get; set; }
        public string BranchName { get; set; }
        public DateTime CommisionDate { get; set; }
        public string Cli_code { get; set; }
        public string Cli_Adr_Code { get; set; }
        public string CliAdrNewCode { get; set; }
        public string EngName { get; set; }
        public string TicketFollowUp { get; set; }
        public string MediaName { get; set; }
        public string Pending_for_team { get; set; }
        public string brAreaGroup { get; set; }
        public string brSupportOffice { get; set; }
        public string brArea { get; set; }
        public string cli_category { get; set; }
        public string PendingReson { get; set; }
        public string brAddress { get; set; }
        public string phone_no { get; set; }
        public string Emp_ID { get; set; }
        public string Emp_Name { get; set; }
        public string Team_Name { get; set; }
        public string TeamAssignDate { get; set; }
        public string AssignBy { get; set; }
        public string Status { get; set; }
        public Int32 TeamAssignSlNo { get; set; }


    }
}
