using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class View_DarkClient_ColorViewModel
    {
        public int autoid { get; set; }
        public string? CoreName { get; set; }
        public string? ClientCode { get; set; }
        public int? ClientSl { get; set; }
        public string? StartPoint { get; set; }
        public string? CableType { get; set; }
        public int? TubeColor { get; set; }
        public int? CoreColor { get; set; }
        public string? CableID { get; set; }
        public string? StartMeter { get; set; }
        public string? EndMeter { get; set; }
        public string? Length { get; set; }
        public string? EndPoint { get; set; }
        public string? CableJoin { get; set; }
        public string? Remarks { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Position { get; set; }
        public string? TubeColorName { get; set; }
        public string? CoreColorName { get; set; }
        public string? ColorRemarks { get; set; }
    }
}
