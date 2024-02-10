using Elasticsearch.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class SP_ComplainTicketInformationAPIModel
    {
        [Key]
        public string? brAdrCode { get; set; }
        public string? DistributorSubscriberCode {get; set;}
        public string? RSMToMISMigratePassword { get; set; }        
        public string? DistributorName {get; set;} 
		public string? RSMToMISMigrateID {get; set;}
        public string? RadiusExpiredDate { get; set; }
        public string? RSMToMISMigrateRadiusPassword { get; set; }
        public int? DistributorID {get; set;}
		public decimal? Amount { get; set;}
		public string? RRPName {get; set;}
        public string? RRPSubscriberCode { get; set; }
        public string? note_for_bts { get; set; }         
		public string? ClientGroup  {get; set;}
		public decimal? SecurityDeposit {get; set;}
		public string? Reference {get; set;}
		public string? ClientName { get; set; }
        public string? BranchName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPersonDesignation { get; set; }
        public string? ContactNumber { get; set; }
        public string? ContactMail { get; set; }
        public string? AddressLine_1 { get; set; }
        public string? AddressLine_2 { get; set; }
        public string? ParentArea { get; set; }
        public string? Area { get; set; }
        public string? WebSite { get; set; }
        public string? StatusOfSLA { get; set; }
        public DateTime? SLADate { get; set; }
        public DateTime? DateOfInception { get; set; }
        public string? ClientCategory { get; set; }
        public string? RevenueCategory { get; set; }
        public string? BusinessType { get; set; }
        public string? SupportOffice { get; set; }
        public string? sta {get; set;} 

        public string? MrtgLink {get; set;}
		public string? Installationby {get; set;}
		public DateTime? ThirdpartyInstallationDate {get; set;}
		public string? BillingAddress {get; set;}
		public string? BillDeliveryOption {get; set;} 
		public string? Bandwidth_CIR {get; set;}
		public string? Bandwidth_MIR {get; set;} 
		public string? Note_For_BW {get; set;} 
		public string? ITC_Bandwidth_DownP_CIR {get; set;} 
		public string? ITC_Bandwidth_DownP_MIR {get; set;}
		public string? ITC_Bandwidth_UP_CIR {get; set;} 
		public string? ITC_Bandwidth_UP_MIR {get; set;} 
		public string? Connectivity_Media {get; set;}
        public int? SdStatus { get; set; }

        public string? PublicIP { get; set; }
        public string? Gateway { get; set; }
        public string? SubnetMask { get; set; }
        public string? PrivateIP { get; set; }
        public string? PrivateGateway { get; set; }
        public string? PrivateSubnetMask { get; set; }
        public string? DNS1 { get; set; }
        public string? DNS2 { get; set; }
        public string? SMTP { get; set; }
        public string? POP3 { get; set; }
        public string? RouterSwitchIP { get; set; }
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Passward { get; set; }
        public string? Catagory { get; set; }
        public string? ServiceNarration { get; set; }
        public string? ItemName { get; set; }
        public decimal? BillingAmount { get; set; }
        public string? VatName { get; set; }
        public int? ScopeID { get; set; }
        public string? LockUnlock { get; set; }
        public string? ServiceStatus { get; set; }
        public DateTime? subcribeExpireDate { get; set; }
        public string SuccessOrErrorMessage { get; set; }


    }
}
