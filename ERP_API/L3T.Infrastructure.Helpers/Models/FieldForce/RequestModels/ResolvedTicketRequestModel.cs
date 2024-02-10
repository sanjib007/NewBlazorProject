using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class ResolvedTicketRequestModel
    {
        public string TicketRefNo { get; set; }
        public string SupportType { get; set; }
        public bool? ChangeAccessPermission { get; set; }
        public DateTime AccessPermissionDateTime { get; set; }
        public bool? CheckboxClosingNature { get; set; }
        public string ClosingNature { get; set; }
        public string? ResonForOutage { get; set; }
        public bool? CheckBoxRFOAddToMail { get; set; }
        public string? ResonForSupportDelay { get; set; }
        public string Comments { get; set; }
        public DateTime DPTResolveDateTime { get; set; }
        public DateTime? CheckboxISRN { get; set; }
        public string? TXTMobileNo { get; set; }
        public bool? CheckboxSendSMS { get; set; }
        public bool? CheckboxEmailTo { get; set; }
        public string? CustomerToEmails { get; set; }
        public string? CustomerCCEmailsCC { get; set; }
        public string? TxtMailTemplate { get; set; }
        public bool? CheckEngneer { get; set; }
        public List<string>? SelectedEmployeeListBox { get; set; }
        public bool? CheckboxResolveDetailsMail { get; set; }
        public string? TxtMailToTemplate { get; set; }
        public string? TxtMailToTemplateCC { get; set; }
        public string? TxtTemplate2 { get; set; }
    }
}
