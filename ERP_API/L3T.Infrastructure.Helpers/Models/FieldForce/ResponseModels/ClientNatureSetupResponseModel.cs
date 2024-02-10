using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientNatureSetupResponseModel
    {
        [Key]
        public int NatureTypeID { get; set; }
        public string? NatureTypeName { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifyingUserID { get; set; }
    }
}
