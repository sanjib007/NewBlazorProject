using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientDatabaseMediaSetupResponseModel
    {
        [Key]
        public Int32 MedID { get; set; }
        public Int32? TechID { get; set; }
        public string? MediaName { get; set; }
    }
}
