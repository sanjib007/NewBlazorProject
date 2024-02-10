namespace L3TIdentityServer.Models.Account
{
    public class GetUserRolesRequest : PaginationModel
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public bool PagingMode { get; set; } = false;
    }
}
