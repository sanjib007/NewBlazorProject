using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS;
    [Keyless]
    public class ClientDatabaseMain
    {
        public string? brCliCode { get; set; }
        public int? brSlNo { get; set; }
        public string? brAdrCode { get; set; }
        public string? brAdrNewCode { get; set; }
        public string? brName { get; set; }
        public int? brGroupId { get; set; }
        public string? brGroup { get; set; }
        public string? brMktGrpId { get; set; }
        public string? brMktGroup { get; set; }
        public int? brClientTypeId { get; set; }
        public string? brClientType { get; set; }
        public int? brAddressTypeID { get; set; }
        public string? brAddressTypeName { get; set; }
        public int? brBusinessTypeId { get; set; }
        public string? brBusinessType { get; set; }
        public int? brCategoryId { get; set; }
        public string? brCategory { get; set; }
        public int? brNatureId { get; set; }
        public string? brNature { get; set; }
        public int? brStatus { get; set; }
        public int? brInsStatus { get; set; }
        public int? brCrStatus { get; set; }
        public int? brBlStatus { get; set; }
        public int? brSearch { get; set; }
        public int? SdStatus { get; set; }
        public DateTime? brBlStdate { get; set; }
        public DateTime? brLastBlDate { get; set; }
        public string? brAddress1 { get; set; }
        public string? brAddress2 { get; set; }
        public int? brAreaGroupId { get; set; }
        public string? brAreaGroup { get; set; }
        public int? brAreaId { get; set; }
        public string? brArea { get; set; }
        public string? brPostalArea { get; set; }
        public string? brwebsite { get; set; }
        public int? brstatussla { get; set; }
        public DateTime? brsladate { get; set; }
        public DateTime? brdateinception { get; set; }
        public string? brcompanyname { get; set; }
        public string? branchmanager { get; set; }
        public int? brSupportOfficeId { get; set; }
        public string? brSupportOffice { get; set; }
        public string? contact_det { get; set; }
        public string? Contact_Designation { get; set; }
        public string? phone_no { get; set; }
        public string? fax_no { get; set; }
        public string? email_id { get; set; }
        public string? mrtg_link { get; set; }
        public string? note_for_services { get; set; }
        public string? bw_as_client { get; set; }
        public string? add_for_p2p { get; set; }
        public string? note_for_bts { get; set; }
        public string? cur_status { get; set; }
        public string? final_update_from { get; set; }
        public string? final_update_by { get; set; }
        public DateTime? final_update_date { get; set; }
        public DateTime? i_bill_date { get; set; }
        public string? i_seller { get; set; }
        public string? i_acc_manager { get; set; }
        public string? i_ins_ref_no { get; set; }
        public DateTime? i_ins_date { get; set; }
        public string? i_ins_engg { get; set; }
        public string? i_bill_mgr { get; set; }
        public string? i_terms_cond { get; set; }
        public int? sll { get; set; }
        public string? UpdStatus { get; set; }
        public int? ClientUpdStatus { get; set; }
        public int? sdststemp { get; set; }
        public string? HeadOfficeName { get; set; }
        public string? BranchName { get; set; }
        public string? LockUnlock { get; set; }
        public string? SMSMobileNo { get; set; }
        public string? ClientRefarence { get; set; }
        public string? NewE1POP { get; set; }
        public string? VPNTunnel { get; set; }
        public string? ICBWProvider { get; set; }
        public DateTime? SofDate { get; set; }
        public string? SubscriberType { get; set; }
        public string? Salution { get; set; }
        public string? GenderType { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }
        public string? Occupation { get; set; }
        public string? MQStatus { get; set; }
        public string? BillDeliverd { get; set; }
        public string? MqID { get; set; }
        public string? MqActiveInactive { get; set; }
        public string? SubscriberPassword { get; set; }
        public string? SOFCompleteBy { get; set; }
        public DateTime? SOFCompleteDate { get; set; }
        public string? DistributorID { get; set; }
        public string? PackagePlan { get; set; }
        public string? DistributorSubscriberID { get; set; }
        public int? RRPID { get; set; }
        public string? RRPSubscriberID { get; set; }
        public int? SMEPkgID { get; set; }
        public int? DisPackageID { get; set; }
        public int? TimeBased { get; set; }
        public string? SAFNumber { get; set; }
        public int? ProofTypeID { get; set; }
        public string? ProofID_No { get; set; }
    }

