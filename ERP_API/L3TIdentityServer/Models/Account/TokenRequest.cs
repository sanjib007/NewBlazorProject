using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models.Account
{
    public class TokenRequest
    {
        [MaxLength(100)]
        public string ClientId { get; set; }
        [MaxLength(100)]
        public string ClientSecret { get; set; }
        [MaxLength(100)]
        public string Scope { get; set; }
        [MaxLength(100)]
        public string GrantType { get; set; }
        [MaxLength(100)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
