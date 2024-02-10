using L3T.OAuth2DotNet7.DataAccess.IdentityModels;


namespace L3T.OAuth2DotNet7.Services.Interface
{
    public interface IThirdPartyHttpRequestService
    {
        Task<AppUser> GetUserInformation(string id, string pass);
    }
}
