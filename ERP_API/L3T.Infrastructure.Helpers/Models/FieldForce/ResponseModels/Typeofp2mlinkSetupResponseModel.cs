using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class Typeofp2mlinkSetupResponseModel
    {
        [Key]
        public int Typeofp2mlinkID { get; set; }
        public string? Typeofp2mlink { get; set; }
    }
}
