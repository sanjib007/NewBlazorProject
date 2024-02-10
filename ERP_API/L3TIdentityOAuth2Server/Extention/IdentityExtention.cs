using L3TIdentityOAuth2Server.DataAccess;
using L3TIdentityOAuth2Server.DataAccess.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace L3TIdentityOAuth2Server.Extention
{
    public static class IdentityExtention
    {
        public static IServiceCollection AddIdentityExtention(this IServiceCollection services)
        {
            
            services.AddIdentity<AppUser, AppRoles>(
                    options => {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.Stores.MaxLengthForKeys = 85;
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequiredUniqueChars = 1;
                    }
                )
                .AddEntityFrameworkStores<IdentityTokenServerDBContext>()
                .AddDefaultTokenProviders();
            
            return services;
        }
    }
}