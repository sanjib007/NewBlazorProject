using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data.ChangeRequirementModel
{
    public class StepFourRequestModel
    {
        [Required(ErrorMessage = "Level Of Risk is required")]
        public string LevelOfRisk { get; set; }
        
        [Required(ErrorMessage = "Level Of Risk Description is required")]
        public string LevelOfRiskDescription { get; set; }
        
        [Required(ErrorMessage = "Alternative Description is required")]
        public string AlternativeDescription { get; set; }
    }
}
