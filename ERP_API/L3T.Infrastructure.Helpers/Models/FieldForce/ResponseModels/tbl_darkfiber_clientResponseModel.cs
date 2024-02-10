using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_darkfiber_clientResponseModel
    {
        [Key]
        public int AutoID { get; set; }
        public string? ClientCode { get; set; }
        public int? ClientSl { get; set; }
        public string? ClientName { get; set; }
        public string? LocationName1 { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? LocationName2 { get; set; }
        public string? p2pAddress1 { get; set; }
        public string? p2pAddress2 { get; set; }
        public int? NoOfCore { get; set; }
        public string? CoreName { get; set; }
        public string? CablePathType { get; set; }
        public string? LinkDistance { get; set; }
        public string? LinkPath { get; set; }
        public string? MailComplain { get; set; }
        public string? Remarks { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUserID { get; set; }
    }
}
