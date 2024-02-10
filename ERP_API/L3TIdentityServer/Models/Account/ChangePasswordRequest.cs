using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models.Account
{
    public class ChangePasswordRequest
    {
        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
