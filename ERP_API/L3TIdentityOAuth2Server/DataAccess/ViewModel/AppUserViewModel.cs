using L3TIdentityOAuth2Server.DataAccess.IdentityModels;

namespace L3TIdentityOAuth2Server.DataAccess.ViewModel;

public class AppUserViewModel : AppUser
{
    public List<string> RoleName { get; set; }
}