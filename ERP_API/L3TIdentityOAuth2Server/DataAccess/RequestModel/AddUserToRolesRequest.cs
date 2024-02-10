namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

public class AddUserToRolesRequest
{
    public string UserName { get; set; }
    public IEnumerable<string> roles { get; set; }
}
