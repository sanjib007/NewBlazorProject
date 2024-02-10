using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models.Account
{
    public class LoginRequest
    {
        [MaxLength(100)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
    }
}
