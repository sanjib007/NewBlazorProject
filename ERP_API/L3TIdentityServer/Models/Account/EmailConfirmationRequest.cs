using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models.Account
{
    public class EmailConfirmationRequest
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "-4500")]
        public string Email { get; set; }
    }
}
