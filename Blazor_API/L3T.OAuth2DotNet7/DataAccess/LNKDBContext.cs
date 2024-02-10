using Microsoft.EntityFrameworkCore;

namespace L3T.OAuth2DotNet7.DataAccess
{
    public class LNKDBContext : DbContext
    {
        public LNKDBContext(DbContextOptions options) : base(options)
        {
        }

        //Sql Query view Model
        public DbSet<SearchEmployeeModel> SearchLNKEmployees { get; set; }

        // sp model
        //public virtual DbSet<StatusWiseTotalCrResponse> StatusWiseTotalCrResponse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchEmployeeModel>().HasNoKey().ToView("v_SearchEmployeeModel");
            base.OnModelCreating(modelBuilder);
        }
    }
}
