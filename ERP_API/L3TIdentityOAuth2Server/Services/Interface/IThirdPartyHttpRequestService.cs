using L3TIdentityOAuth2Server.DataAccess.IdentityModels;
using L3TIdentityOAuth2Server.DataAccess.ViewModel;

namespace L3TIdentityOAuth2Server.Services.Interface
{
    public interface IThirdPartyHttpRequestService
    {
        Task<AppUser> GetUserInformation(string id, string pass);
        Task storeAppInformation(string info, string userId);
    }
}
