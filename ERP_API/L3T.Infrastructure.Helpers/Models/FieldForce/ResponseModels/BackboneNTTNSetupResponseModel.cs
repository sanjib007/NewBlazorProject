using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class BackboneNTTNSetupResponseModel
    {
        [Key]
        public int NTTNID { get; set; }
        public string? NTTNName { get; set; }
    }
}
