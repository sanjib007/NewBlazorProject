using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models.Account
{
    public class CreateUserByAdminRequest
    {
        [Required]
        public string UserName { get; set; }

        //[Required]
        //public string FirstName { get; set; }

        //[Required]
        //public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string[] Roles { get; set; }
        public string PhoneNumber { get; set; }
    }
}
