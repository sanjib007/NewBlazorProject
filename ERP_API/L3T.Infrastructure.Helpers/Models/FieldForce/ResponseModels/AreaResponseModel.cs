using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class AreaResponseModel
    {
        [Key]
        public int AreaID { get; set; }
        public int? AreaGroupID { get; set; }
        public string? AreaName { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
    }
}
