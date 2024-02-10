using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using L3T.OAuth2DotNet7.DataAccess.Model;
using L3T.OAuth2DotNet7.DataAccess.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace L3T.OAuth2DotNet7.DataAccess
{
    public class IdentityTokenServerDBContext : IdentityDbContext<AppUser, AppRoles, long,
        IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public IdentityTokenServerDBContext(DbContextOptions<IdentityTokenServerDBContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeePreAssignRole> EmployeePreAssignRoles { get; set; }
        public DbSet<IdentityRequestResponseModel> IdentityRequestResponse { get; set; }
        public DbSet<RSAEncryptDataDuplocationCheckModel> RSAEncryptDataDuplocationCheck { get; set; }
        public DbSet<MenuSetupModel> MenuSetups { get; set; }


        //Sql Query view Model
        public virtual DbSet<GetAllDepartmentModel> GetAllDepartment { get; set; }
        public virtual DbSet<DepartmentWiseEmployeeModel> GetAllEmployee { get; set; }
        public virtual DbSet<AppUserModel> AppUserView { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GetAllDepartmentModel>().HasNoKey().ToView("v_GetAllDepartmentModel");
            base.OnModelCreating(modelBuilder);
        }
    }
}
//dotnet ef migrations add "updateAspNetUserTable"  -p "L3TIdentityOAuth2Server" -c  IdentityTokenServerDBContext
//dotnet ef database update  -p "L3TIdentityOAuth2Server" -c  IdentityTokenServerDBContext

//EntityFrameworkCore\Add-Migration init -Context IdentityTokenServerDBContext 
//EntityFrameworkCore\Update-Database -Context IdentityTokenServerDBContext