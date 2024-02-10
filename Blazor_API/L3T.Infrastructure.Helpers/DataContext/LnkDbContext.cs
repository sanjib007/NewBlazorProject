using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.Parmission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class LnkDbContext : DbContext
    {
        public LnkDbContext(DbContextOptions options) : base(options)
        {
        }

        //Sql Query view Model
        public virtual DbSet<GetAllDepartmentModel> GetAllDepartment { get; set; }
        public virtual DbSet<SearchEmployeeModel> SearchEmployeeModel { get; set; }
        

        // sp model
        //public virtual DbSet<StatusWiseTotalCrResponse> StatusWiseTotalCrResponse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GetAllDepartmentModel>().HasNoKey().ToView("v_GetAllDepartmentModel");
            base.OnModelCreating(modelBuilder);
        }
    }
}
