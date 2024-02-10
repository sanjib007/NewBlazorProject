using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.Parmission;
using L3T.Infrastructure.Helpers.Models.SystemErrorLog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext
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
