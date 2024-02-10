
using L3TIdentityTokenServer.DataAccess.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace L3TIdentityTokenServer.DataAccess
{
    public class IdentityTokenServerDBContext : IdentityDbContext<AppUser, AppRoles, long,
        IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public IdentityTokenServerDBContext(DbContextOptions<IdentityTokenServerDBContext> options)
            : base(options)
        {
        }
    }
}
