namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class AddUserToRolesRequest
{
    public string UserName { get; set; }
    public IEnumerable<string> roles { get; set; }
}
