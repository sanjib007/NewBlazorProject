using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class View_JoinColorResponseModel
    {
        public int? autoid { get; set; }
        public int? BtsID { get; set; }
        public int? OdfID { get; set; }
        public int? Tray { get; set; }
        public int? Port { get; set; }
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
        public string? CableNo { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Position { get; set; }
        public string? TubeColorName { get; set; }
        public string? CoreColorName { get; set; }
        public string? Remarks { get; set; }
        public string? JoinNo { get; set; }
        public int? AutoODFID { get; set; }
        public string? JoinColorRemarks { get; set; }
    }
}
