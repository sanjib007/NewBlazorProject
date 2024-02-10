using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_ODFNameSetUpResponseModel
    {
        [Key]
        public int IncrementID { get; set; }
        public int? BtsID { get; set; }
        public int? ODFID { get; set; }
        public string? ODFName { get; set; }
        public string? CableID { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Remarks { get; set; }
    }
}
