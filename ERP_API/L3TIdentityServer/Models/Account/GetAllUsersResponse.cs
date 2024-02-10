
namespace L3TIdentityServer.Models.Account
{
    public class GetAllUsersResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string Email { get; set; }
        //public string MiddleName { get; set; }
        //public string BirthDate { get; set; }
        public string PhoneNumber { get; internal set; }
        public bool PhoneNumberConfirmed { get; set; }
        //public string ProfileImageUrl { get; internal set; }
        //public bool IsActive { get; internal set; }
        //public bool IsTemporaryCustomer { get; internal set; }
    }
}
