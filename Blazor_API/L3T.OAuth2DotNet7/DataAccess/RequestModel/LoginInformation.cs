using System.Security.Claims;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class LoginInformation
{
    public ClaimsPrincipal User { get; set; }
}