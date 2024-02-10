using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data.ChangeRequirementModel
{
    public class StepTwoRequestModel
    {
        [Required(ErrorMessage = "Change From Existing is required")]
        public string ChangeFromExisting { get; set; }
        
        [Required(ErrorMessage = "Change To After is required")]
        public string ChangeToAfter { get; set; }

        [Required(ErrorMessage = "Change Impact Description is required")]
        public string ChangeImpactDescription { get; set; }

        [Required(ErrorMessage = "Justification is required")]
        public string Justification { get; set; }
    }
}
