using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class SupportOfficeResponseModel
    {
        [Key]
        public Int32 SupportOfficeID { get; set; }
        public string? SupportOfficeName { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifyingUserID { get; set; }

    }
}
