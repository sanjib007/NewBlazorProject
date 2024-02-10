using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models.Account
{
    public class PhoneNumberTokenRequest
    {
        [Required]
        public string PhoneNumber { get; set; }


        [Required]
        public string Username { get; set; }
    }
}
