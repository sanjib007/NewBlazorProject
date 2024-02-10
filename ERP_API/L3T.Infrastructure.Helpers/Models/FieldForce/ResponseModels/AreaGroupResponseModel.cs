using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class AreaGroupResponseModel
    {
        [Key]
        public int AreaGroupID { get; set; }
        public int? DistrictID { get; set; }
        public string? AreaGroupName { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
        public int? SupportOfficeID { get; set; }
        public string? DivisionSetup { get; set; }
    }
}
