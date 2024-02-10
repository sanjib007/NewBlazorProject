using AutoMapper;
using L3TIdentityServer.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace L3TIdentityServer.Models
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            //CreateMap<MB_GetUserByRoleResponse, User>()
            //    .ReverseMap();

            CreateMap<IdentityUser, UserProfileResponse>();

            //CreateMap<UpdateUserProfileRequest, User>();

            //CreateMap<UpdateUserProfileRequest, MB_UserProfileChange>();
        }
    }
}
