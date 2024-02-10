using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientGroupSetupResponseModel
    {
        [Key]
        public int ClientGroupID { get; set; }
        public string? ClientGroupName { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifyingUserID { get; set; }
    }
}
