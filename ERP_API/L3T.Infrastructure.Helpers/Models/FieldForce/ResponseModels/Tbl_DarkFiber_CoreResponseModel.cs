using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class Tbl_DarkFiber_CoreResponseModel
    {
        [Key]
        public int coreid { get; set; }
        public int noofcore { get; set; }
        public string corename { get; set; }
    }
}
