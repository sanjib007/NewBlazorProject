using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data.ChangeRequirementModel
{
    public class StepOneRequestModel
    {
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }
        public string? ChangeRequestFor { get; set; }
    }
}
