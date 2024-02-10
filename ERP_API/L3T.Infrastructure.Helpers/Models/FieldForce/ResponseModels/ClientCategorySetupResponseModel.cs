using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientCategorySetupResponseModel
    {
        [Key]
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? ModifyingUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryUserID { get; set; }
        public int? Status { get; set; }
    }
}
