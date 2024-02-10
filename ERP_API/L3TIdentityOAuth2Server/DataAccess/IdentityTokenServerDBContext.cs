using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.IdentityModels;
using L3TIdentityOAuth2Server.DataAccess.NotificationRelatedModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace L3TIdentityOAuth2Server.DataAccess
{
    public class IdentityTokenServerDBContext : IdentityDbContext<AppUser, AppRoles, long,
        IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public IdentityTokenServerDBContext(DbContextOptions<IdentityTokenServerDBContext> options)
            : base(options)
        {

        }

        public DbSet<AppInfoModel> AppInfos { get; set; }
        public DbSet<IdentityRequestResponseModel> IdentityRequestResponses { get; set; }
    }
}
//dotnet ef migrations add "updateAspNetUserTable"  -p "L3TIdentityOAuth2Server" -c  IdentityTokenServerDBContext
//dotnet ef database update  -p "L3TIdentityOAuth2Server" -c  IdentityTokenServerDBContext