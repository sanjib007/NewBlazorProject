using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class TblComplainInfoModel
    {
        [Key]
        public string? comp_info_ref_no { get; set; }
        public int? AutoSL {  get; set; }
        public string? Clientcategory { get; set; }
        public string? DistributorTaskStatus { get; set; }
        public string? comp_info_Category { get; set; }
        public string? comp_info_Led_Status { get; set; }
        public DateTime? comp_info_Receive_ActualTime { get; set; }
        public DateTime? comp_info_Receive_Date { get; set; }
        public string? comp_info_Receive_Time { get; set; }
        public string? comp_info_Source_Information { get; set; }
        public string? comp_info_attachments { get; set; }
        public string? comp_info_client_Media { get; set; }
        public string? comp_info_client_adr { get; set; }
        public string? comp_info_client_adrcode { get; set; }
        public string? comp_info_client_id { get; set; }
        public string? comp_info_client_name { get; set; }
        public int? comp_info_client_slno { get; set; }
        public string? comp_info_com_name { get; set; }
        public string? comp_info_comm { get; set; }
        public string? comp_info_complain { get; set; }
        public string? comp_info_con_email { get; set; }
        public string? comp_info_con_phone_no { get; set; }
        public string? comp_info_contact_person { get; set; }
        public DateTime? comp_info_deadline { get; set; }
        public string? comp_info_email_to_client { get; set; }
        public string? comp_info_hold_on { get; set; }
        public DateTime? comp_info_last_update { get; set; }
        public string? comp_info_manually_email { get; set; }
        public string? comp_info_mkt_person { get; set; }
        public string? comp_info_postponed_flg { get; set; }
        public int? comp_info_postponed_hour { get; set; }
        public DateTime? comp_info_postponed_time { get; set; }
        public string? comp_info_receive_by { get; set; }
        public string? comp_info_related_dept { get; set; }
        public string? comp_info_resolve_status { get; set; }
        public string? comp_info_service_code { get; set; }
        public string? comp_info_service_desc { get; set; }
        public string? comp_info_type { get; set; }
        public string? receiveby { get; set; }
        public int state { get; set; }
    }
}
