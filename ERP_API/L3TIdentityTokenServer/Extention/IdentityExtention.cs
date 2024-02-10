using L3TIdentityTokenServer.DataAccess;
using L3TIdentityTokenServer.DataAccess.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace L3TIdentityTokenServer.Extention
{
    public static class IdentityExtention
    {
        public static IServiceCollection AddIdentityExtention(this IServiceCollection services)
        {
            
            services.AddIdentity<AppUser, AppRoles>(
                    options => {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.Stores.MaxLengthForKeys = 85;
                        options.Password.RequireDigit = true;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.Password.RequireUppercase = true;
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