using L3T.OAuth2DotNet7.DataAccess.Model.Parmission;
using Microsoft.EntityFrameworkCore;

namespace L3T.OAuth2DotNet7.DataAccess
{
    public class ApplicationMenuAndRoleWiseMenuPermissionDBContext : DbContext
    {
        public ApplicationMenuAndRoleWiseMenuPermissionDBContext(DbContextOptions<ApplicationMenuAndRoleWiseMenuPermissionDBContext> options) : base(options)
        {
        }

        public DbSet<MenuSetupModel> MenuSetup { get; set; }
        public DbSet<RoleWiseMenuPermissionModel> RoleWiseMenuPermission { get; set; }



        //Sql Query view Model
        public virtual DbSet<MenuSetupAndPermissionViewModel> MenuSetupAndPermissionView { get; set; }


        // sp model
        //public virtual DbSet<StatusWiseTotalCrResponse> StatusWiseTotalCrResponse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuSetupAndPermissionViewModel>().HasNoKey().ToView("v_MenuSetupAndPermissionView");
            base.OnModelCreating(modelBuilder);
        }

    }
}
