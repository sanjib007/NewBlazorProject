using System.Security.Claims;

namespace L3TIdentityTokenServer.DataAccess.RequestModel;

public class LoginInformation
{
    public ClaimsPrincipal User { get; set; }
}