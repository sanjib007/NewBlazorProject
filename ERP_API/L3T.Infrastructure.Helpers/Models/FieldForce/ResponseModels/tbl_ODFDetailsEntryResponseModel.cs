using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_ODFDetailsEntryResponseModel
    {
        [Key]
        public int IncrementID { get; set; }
        public int? BtsID { get; set; }
        public int? ODFNameID { get; set; }
        public int? TrayID { get; set; }
        public int? PortID { get; set; }
        public int? TubeID { get; set; }
        public int? ColorID { get; set; }
        public string? JoinColor { get; set; }
        public string? ClientBackBoneName { get; set; }
        public string? Address { get; set; }
        public string? GooglePath { get; set; }
        public string? Remarks { get; set; }
        public string? ODFFrom { get; set; }
        public string? ODFTo { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? AssignEng { get; set; }
        public DateTime? AssignEngDate { get; set; }
        public string? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Status { get; set; }
        public string? ClientCode { get; set; }
        public int? ClientSl { get; set; }
        public int? NTTNNameID { get; set; }
        public string? FiberSCRID { get; set; }
        public string? SummitLinkID { get; set; }
        public string? BahonCoreID { get; set; }
        public int? CablePathTypeID { get; set; }
        public int? BackboneID { get; set; }
    }
}
