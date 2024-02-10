using IPV6ConfigSetup.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace IPV6ConfigSetup.DataAccess
{
    public class IPV6ConfigSetupDBContext : DbContext
    {
        public IPV6ConfigSetupDBContext(DbContextOptions<IPV6ConfigSetupDBContext> options)
            : base(options)
        {
        }
        public DbSet<IPV6_PrimarySubnetModel> IPV6_PrimarySubnet { get; set; }
        public DbSet<IPV6_DivisionSubnet32Model> IPV6_DivisionSubnet32 { get; set; }
        public DbSet<IPV6_UserTypeSubnet36Model> IPV6_UserTypeSubnet36 { get; set; }
        public DbSet<IPV6_CitySubnet44Model> IPV6_CitySubnet44 { get; set; }
        public DbSet<IPV6_BTSSubnet48Model> IPV6_BTSSubnet48 { get; set; }
        public DbSet<IPV6_ParentSubnet56Model> IPV6_ParentSubnet56 { get; set; }
        public DbSet<IPV6_CustomerSubnet64Model> IPV6_CustomerSubnet64 { get; set; }



        //Sql Query view Model
        //public virtual DbSet<GetAllDepartmentModel> GetAllDepartment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<GetAllDepartmentModel>().HasNoKey().ToView("v_GetAllDepartmentModel");
            base.OnModelCreating(modelBuilder);
        }
    }
}
