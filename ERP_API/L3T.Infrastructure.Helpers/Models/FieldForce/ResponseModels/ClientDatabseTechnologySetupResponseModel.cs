using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientDatabseTechnologySetupResponseModel
    {
        [Key]
        public Int32 TechID { get; set; }
        public Int32? WireID { get; set; }
        public string? TechnologyName { get; set; }

    }
}
