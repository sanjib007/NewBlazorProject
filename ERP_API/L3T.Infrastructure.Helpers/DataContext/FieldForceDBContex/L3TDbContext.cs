using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex
{

    public class L3TDbContext : DbContext
    {
        public L3TDbContext(DbContextOptions<L3TDbContext> options) : base(options)
        {
        }

        public DbSet<tbl_InTrTrnHdrModel> InTrTrnHdrModel { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
