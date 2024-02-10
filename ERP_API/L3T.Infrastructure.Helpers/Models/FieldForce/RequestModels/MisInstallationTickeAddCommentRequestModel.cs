using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class MisInstallationTickeAddCommentRequestModel
    {
        public string TicketNo { get; set; }
        public string AddComments { get; set; }
        public string PendingReasonText { get; set; }
        public int PendingReasonValue { get; set; }
        public string companyName { get; set; }
        public List<string>? chkpenteam { get; set; }
        public string AdditionalMail { get; set;}

        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserCellNo { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? Ip { get; set; }

    }
}
