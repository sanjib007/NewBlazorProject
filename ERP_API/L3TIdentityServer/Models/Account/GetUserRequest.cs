namespace L3TIdentityServer.Models.Account
{
    public class GetUserRequest : PaginationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// filter on all columns
        /// </summary>
        public string Filter { get; set; }
    }
}
