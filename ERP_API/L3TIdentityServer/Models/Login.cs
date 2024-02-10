using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models
{
    public class Login
    {
        [Required]
        //[EmailAddress]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
