using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientDatabaseWireSetupResponseModel
    {
        [Key]
        public Int32 WireID { get; set; }
        public string? WireName { get; set; } = string.Empty;
    }
}
