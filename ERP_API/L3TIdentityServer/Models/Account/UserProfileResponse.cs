using System;

namespace L3TIdentityServer.Models.Account
{
    public class UserProfileResponse : ResponseModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
