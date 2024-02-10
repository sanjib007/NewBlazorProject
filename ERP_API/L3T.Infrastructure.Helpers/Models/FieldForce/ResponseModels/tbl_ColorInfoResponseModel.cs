using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_ColorInfoResponseModel
    {
        [Key]
        public int ColorID { get; set; }
        public string? ColorName { get; set; }
    }
}
