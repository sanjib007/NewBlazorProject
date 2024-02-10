namespace L3TIdentityTokenServer.DataAccess.RequestModel;

public class AddUserToRolesRequest
{
    public string UserName { get; set; }
    public IEnumerable<string> roles { get; set; }
}