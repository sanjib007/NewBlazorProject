using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.MISInstallation.ResponseModel
{
    public class SubscriptionInfoResponse
    {
        [Key]
        public string Cli_code { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? CommisionDate { get; set; }
        public string? EntryDate { get; set; }
        public string? SalesPerson { get; set; }
        public string? Mname { get; set; }
        public string? Shortdocuments { get; set; }
        public string? Initiator { get; set; }
        public string? BranchName { get; set; }
        public string? BtsName { get; set; }
        public string? Cli_Adr_Code { get; set; }
        public string? InsService { get; set; }
        public string? InStatus { get; set; }
        public string? contact_det { get; set; }
        public string? Contact_Designation { get; set; }
        public string? phone_no { get; set; }
        public string? email_id { get; set; }
        public string? brAddress1 { get; set; }
        public string? brAddress2 { get; set; }
        public string? brAreaGroup { get; set; }
        public string? brArea { get; set; }
        public string? brwebsite { get; set; }
        public string? ClientRefarence { get; set; }
        public string? MqID { get; set; }
        public string? brstatussla { get; set; }
        public DateTime? brsladate { get; set; }
        public DateTime? brdateinception { get; set; }
        public string? brcompanyname { get; set; }
        public string? branchmanager { get; set; }
        public string? brMktGroup { get; set; }
        public string? brCategory { get; set; }
        public string? brNature { get; set; }
        public string? brBusinessType { get; set; }
        public string? mrtg_link { get; set; }
        public string? brCliCode { get; set; }
        public string? brAdrCode { get; set; }
        public int? brSlNo { get; set; }
        public DateTime? i_ins_date { get; set; }
        public DateTime? i_bill_date { get; set; }
        public string? i_seller { get; set; }
        public string? brAdrNewCode { get; set; }

        [NotMapped]
        public string? billingAddress { get; set; }
        [NotMapped]
        public string? p2p_brName { get; set; }
        [NotMapped]
        public string? p2p_contact_det { get; set; }
        [NotMapped]
        public string? p2p_phone_no { get; set; }
        [NotMapped]
        public string? p2p_email_id { get; set; }
        [NotMapped]
        public string? p2p_brAddress1 { get; set; }
        [NotMapped]
        public string? p2p_brAddress2 { get; set; }
        [NotMapped]
        public string? p2p_brAreaGroup { get; set; }
        [NotMapped]
        public string? p2p_brArea { get; set; }

    }
}
