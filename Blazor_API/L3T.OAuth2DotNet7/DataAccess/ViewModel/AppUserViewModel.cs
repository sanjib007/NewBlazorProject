using L3T.OAuth2DotNet7.DataAccess.IdentityModels;

namespace L3T.OAuth2DotNet7.DataAccess.ViewModel;

public class AppUserViewModel : AppUser
{
    public List<string> RoleName { get; set; }
}