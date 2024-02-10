namespace L3TIdentityServer.Models.Account
{
    public class UpdateUserProfileRequest
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
