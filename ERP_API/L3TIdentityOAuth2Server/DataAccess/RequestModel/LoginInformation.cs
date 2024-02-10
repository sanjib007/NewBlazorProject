using System.Security.Claims;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

public class LoginInformation
{
    public ClaimsPrincipal User { get; set; }
}