using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class GetAllPendingTicketByAssignUserResponseModel
    {
        [Key]
        public string? RefNo { get; set; }
        public string? comp_info_client_id { get; set; }
        public int? comp_info_client_slno { get; set; }
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? phone_no { get; set; }
        public string? brAddress1 { get; set; }
        public string? ComplainCategory { get; set; }
        public DateTime? TicketGenerateDate { get; set; }
        public string? Team_Name { get; set; }
        public string? Area { get; set; }
        public string? SupportOffice { get; set; }
        public string? comp_info_client_Media { get; set; }
        public string? Cmplain { get; set; }
        public string? comp_info_receive_by { get; set; }
        public string? comp_info_comm { get; set; }
        public DateTime? comp_info_last_update { get; set; }
        public string? Clientcategory { get; set; }
        public string? SupportType { get; set; }
        public string? BTSName { get; set; }
        public string? comp_info_resolve_status { get; set; }
        public string? AssignEngineer { get; set; }
        public string? LastComments { get; set; }
        public string? PendingReason { get; set; }
        public string? tt { get; set; }
        public string? Team_id { get; set; }
        public string? DistributorName { get; set; }
        public string? CommisionRate { get; set; }
        public string? receiveby { get; set; }
        public string? user_name { get; set; }
        public string? comp_info_Source_Information { get; set; }
        public string? forwadhistory { get; set; }

    }
}
