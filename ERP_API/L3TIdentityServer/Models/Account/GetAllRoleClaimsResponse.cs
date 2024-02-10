namespace L3TIdentityServer.Models.Account
{
    public class GetAllRoleClaimsResponse
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
