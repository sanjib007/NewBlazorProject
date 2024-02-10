using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
    public class Tbl_complain_info
    {
      
        public string? comp_info_type { get; set; }
        [Key]       
        public string? comp_info_ref_no { get; set; }
        public string? comp_info_com_name { get; set; }
        public string? comp_info_client_id { get; set; }
        public int? comp_info_client_slno { get; set; }
        public string? comp_info_client_adrcode { get; set; }
        public string? comp_info_client_name { get; set; }
        public string? comp_info_client_adr { get; set; }
        public string? comp_info_contact_person { get; set; }
        public string? comp_info_con_phone_no { get; set; }
        public string? comp_info_con_email { get; set; }
        public string? comp_info_email_to_client { get; set; }
        public DateTime? comp_info_Receive_Date { get; set; }
        public string? comp_info_Receive_Time { get; set; }
        public DateTime? comp_info_Receive_ActualTime { get; set; }
        public string? comp_info_service_code { get; set; }
        public string? comp_info_service_desc { get; set; }
        public string? comp_info_Category { get; set; }
        public string? comp_info_complain { get; set; }
        public string? comp_info_client_Media { get; set; }
        public string? comp_info_Source_Information { get; set; }
        public string? comp_info_Led_Status { get; set; }
        public DateTime? comp_info_deadline { get; set; }
        public string? comp_info_related_dept { get; set; }
        public string? comp_info_manually_email { get; set; }
        public string? comp_info_receive_by { get; set; }
        public string? comp_info_mkt_person { get; set; }
        public string? comp_info_comm { get; set; }
        public string? comp_info_attachments { get; set; }
        public string? comp_info_resolve_status { get; set; }
        public DateTime? comp_info_last_update { get; set; }
        public DateTime? comp_info_postponed_time { get; set; }
        public string? comp_info_postponed_flg { get; set; }
        public int? comp_info_postponed_hour { get; set; }
        public string? comp_info_hold_on { get; set; }
        public int? state { get; set; }
        public string? receiveby { get; set; }
        public string? Clientcategory { get; set; }
        public string? DistributorTaskStatus { get; set; }
        public string? SupportType { get; set; }
        public string? ScheduleDate { get; set; }
        public string? AssignEngineer { get; set; }
        public string? LastComments { get; set; }
        public string? OpenCategory { get; set; }
        public string? CallerID { get; set; }
        public int? PendingReason { get; set; }
        public DateTime? StatusChangeDateTime { get; set; }
        public string? IsBusinessHour { get; set; }
        public long? OutageReson { get; set; }
        public long? DelayReson { get; set; }
        public string? MonitoringType { get; set; }
        public DateTime? MonitoringFaultOccuranceTime { get; set; }
        public string? MehodName { get; set; }
        public string? NewSupportType { get; set; }
        public int? OpeningCategoryID { get; set; }
        public int? OpeningNatureID { get; set; }
        public int? OpeningTaskCategoryID { get; set; }
       
        public int? AutoSL { get; set; }
    }
}
