namespace L3TIdentityServer.Models.Account
{
    public class GetRoleRequest:PaginationModel
    {
        public string RoleName { get; set; }
        public bool PagingMode { get; set; }
    }
}
