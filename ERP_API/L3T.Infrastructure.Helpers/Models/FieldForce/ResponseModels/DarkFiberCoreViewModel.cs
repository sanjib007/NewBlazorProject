using System.ComponentModel.DataAnnotations;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class DarkFiberCoreViewModel
    {
        [Key]
        public int coreid { get; set; }
        public int noofcore { get; set; }
        public string corename { get; set; }
        public List<View_DarkClient_ColorViewModel>? View_DarkClient_ColorsList { get; set; }
    }
}
