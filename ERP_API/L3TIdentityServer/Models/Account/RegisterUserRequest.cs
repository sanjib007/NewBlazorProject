using System;
using System.ComponentModel.DataAnnotations;

namespace L3TIdentityServer.Models.Account
{
    public class RegisterUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "-4500")]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public string DateOfBirth { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; }

        public string ProfileImageUrl { get; set; }
        public bool IsTemporaryCustomer { get; set; }
        public bool IsActive { get; set; }
    }
}
