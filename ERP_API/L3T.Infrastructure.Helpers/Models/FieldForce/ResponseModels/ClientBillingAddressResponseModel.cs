using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class ClientBillingAddressResponseModel
    {
        public string? br_cli_code { get; set; }
        public int? br_sl_no { get; set; }
        public string? BrAdrCode { get; set; }
        public string? br_name { get; set; }
        public string? br_contact_name { get; set; }
        public string? br_contact_num { get; set; }
        public string? br_contact_email { get; set; }
        public string? br_adr1 { get; set; }
        public string? br_adr2 { get; set; }
        public string? br_area { get; set; }
        public string? br_sub_area { get; set; }
        public string? br_postal_area { get; set; }
        public int? UpdStatus { get; set; }
        public string? RoadNo { get; set; }
        public string? Sector { get; set; }
        public string? Block { get; set; }
        public string? Division { get; set; }
        public string? District { get; set; }
        public string? Thana { get; set; }
        public string? ParentArea { get; set; }
        public string? SubArea { get; set; }
        public string? PostalCode { get; set; }
        public string? LandMark { get; set; }
        public string? BuildingName { get; set; }
    }
}
