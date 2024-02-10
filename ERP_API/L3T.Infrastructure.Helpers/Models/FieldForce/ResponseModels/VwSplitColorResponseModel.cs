using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class VwSplitColorResponseModel
    {
        public string? TubeColorName { get; set; }
        public string? CoreColorName { get; set; }
        [Key]
        public int autoid { get; set; }
        public int? BtsID { get; set; }
        public string? OltName { get; set; }
        public int? PON { get; set; }
        public int? Port { get; set; }
        public string? SplitterName { get; set; }
        public string? CustomerID { get; set; }
        public string? StartPoint { get; set; }
        public string? CableType { get; set; }
        public int? TubeColor { get; set; }
        public int? CoreColor { get; set; }
        public string? CableID { get; set; }
        public string? StartMeter { get; set; }
        public string? EndMeter { get; set; }
        public string? Length { get; set; }
        public string? EndPoint { get; set; }
        public string? Remarks { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Position { get; set; }
        public int? CustomerSl { get; set; }
        public string? Shifted { get; set; }
        //public string JoinColorRemarks { get; set; }

    }
}
