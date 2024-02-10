using AutoMapper;
using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using L3T.OAuth2DotNet7.DataAccess.ViewModel;

namespace L3T.OAuth2DotNet7.Helper;

public class AppMapping : Profile
{
    public AppMapping()
    {
        CreateMap<AppUser, AppUserViewModel>().ReverseMap();
    }
}