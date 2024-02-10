using AutoMapper;
using L3TIdentityOAuth2Server.DataAccess.IdentityModels;
using L3TIdentityOAuth2Server.DataAccess.ViewModel;

namespace L3TIdentityOAuth2Server.Helper;

public class AppMapping : Profile
{
    public AppMapping()
    {
        CreateMap<AppUser, AppUserViewModel>().ReverseMap();
    }
}