﻿using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class Post_MainInsResponseModel
    {
        [Key]
        public int RefID { get; set; }
        public string? RefNO { get; set; }
        public string? ClientAdrNewCode { get; set; }
        public string? Cli_code { get; set; }
        public int? Cli_sl { get; set; }
        public string? Cli_adr_code { get; set; }
        public string? BranchCode { get; set; }
        public int? ServiceID { get; set; }
        public string? BillingType { get; set; }
        public string? VatProcess { get; set; }
        public string? BandwidthCIR { get; set; }
        public string? BandwidthMIR { get; set; }
        public string? BandwidthType { get; set; }
        public string? VsatBandwidthDownCir { get; set; }
        public string? VsatBandwidthDownMir { get; set; }
        public string? VsatBandwidthUpCir { get; set; }
        public string? VsatBandwidthUpMir { get; set; }
        public string? NoteBandwith { get; set; }
        public int? ConnectivityMediaID { get; set; }
        public decimal? Otc { get; set; }
        public decimal? Mrc { get; set; }
        public int? BillingCycleID { get; set; }
        public int? BtsID { get; set; }
        public string? HostedSpace { get; set; }
        public string? ServerLocation { get; set; }
        public string? AntivirusName { get; set; }
        public string? NoOfUser { get; set; }
        public string? ServerType { get; set; }
        public string? DomainName { get; set; }
        public decimal? OtcIntercity { get; set; }
        public decimal? MrcIntercity { get; set; }
        public string? TowerType { get; set; }
        public string? TowerHeight { get; set; }
        public string? Comments { get; set; }
        public string? DocumentsShortez { get; set; }
        public int? PackageID { get; set; }
        public int? NoOfLines { get; set; }
        public int? PulseID { get; set; }
        public string? HostedDomainName { get; set; }
        public string? SllDomainName { get; set; }
        public string? CertificateVendorName { get; set; }
        public string? HostedType { get; set; }
        public string? HostedServerConfiguration { get; set; }
        public DateTime? CommencementDate { get; set; }
        public DateTime? Expiredate { get; set; }
        public string? TecnicalComments { get; set; }
        public string? DomainType { get; set; }
        public string? SSLCertificateName { get; set; }
        public string? HostedServerOS { get; set; }
        public string? HostedMonthlyTraffic { get; set; }
        public string? WebSiteType { get; set; }
        public string? WebApplicationType { get; set; }
        public string? WebProjectName { get; set; }
        public string? TowerOwner { get; set; }
        public string? TowerMaintenancePeriod { get; set; }
        public string? GroundingCableLength { get; set; }
        public string? HardwareWarrantyPeriod { get; set; }
        public string? HardwareModel { get; set; }
        public string? HardwareBrand { get; set; }
        public string? HardwareName { get; set; }
        public string? SMSCategory { get; set; }
        public string? SMSSlab { get; set; }
        public decimal? SMSRate { get; set; }
        public int? TotalSMS { get; set; }
        public string? Validity { get; set; }
        public string? StartDate { get; set; }
        public int? AutoslNo { get; set; }
        public string? ContentDevelopment { get; set; }
        public string? LanItemName { get; set; }
        public int? LanQty { get; set; }
        public string? NoOfYear { get; set; }
        public string? DomainExtension { get; set; }
        public string? DomainRegister { get; set; }
        public decimal? Domian_Hosting_SSL_Cost_BDT { get; set; }
        public decimal? Domian_Hosting_SSL_Cost_USD { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? HostingDatacenter { get; set; }
        public string? HostingServerIP { get; set; }
        public string? HostingServerID { get; set; }
        public string? AllocatedSpace { get; set; }
        public string? PurchaseSpace { get; set; }
        public string? HostingProviderName { get; set; }
        public int? SSLQty { get; set; }
        public decimal? SSLUnitPrice { get; set; }
        public string? SSLDomain2 { get; set; }
        public string? PaymentGatewayOTCType { get; set; }
        public string? PaymentStatus { get; set; }
        public string? NHEWarranty { get; set; }
        public string? ContactPeriod { get; set; }
        public string? NameOfSoftware { get; set; }
        public string? LicenseNumber { get; set; }
        public string? LicenseType { get; set; }
        public string? Intranet_P2P { get; set; }
        public int? PkgID { get; set; }
        public string? PkgServiceCode { get; set; }
    }
}
