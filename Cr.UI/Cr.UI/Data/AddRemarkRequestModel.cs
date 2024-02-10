using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data
{
    public class AddRemarkRequestModel
    {
        public long CrId { get; set; }
        [Required(ErrorMessage = "Remark is required")]
        public string? Remark { get; set; }
    }
}
