using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class View_SubSplliterResponseModel
    {
        public int? SpliterID { get; set; }
        public int? BtsID { get; set; }
        public string? OltName { get; set; }
        public int? PON { get; set; }
        public int? Port { get; set; }
        public int? PortCapacity { get; set; }
        public int? SpliterCapacity { get; set; }
        public string? SpliterLocation { get; set; }
        public string? CustomerName { get; set; }
        public string? CableNo { get; set; }
        public string? LinkPath { get; set; }
        public string? Remarks { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CustomerCode { get; set; }
        public int? CustomerSl { get; set; }
        public int? SdStatus { get; set; }
        public string? Shifted { get; set; }
        public string? BtsSetupName { get; set; }
        public string? EncloserNo { get; set; }
        public string? OLTBrand { get; set; }
        public string? UTPClient { get; set; }
        public string? MqID { get; set; }
        public string? MqActiveInactive { get; set; }
        public string? brCategory { get; set; }
        public string? HeadOfficeName { get; set; }
        public string? BranchName { get; set; }
        public string? email_id { get; set; }
        public string? phone_no { get; set; }
        public string? brAddress1 { get; set; }
        public string? brAreaGroup { get; set; }
    }
}
