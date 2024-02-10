using IPV6ConfigSetup.DataAccess.Model.MISDBModel;
using Microsoft.EntityFrameworkCore;

namespace IPV6ConfigSetup.DataAccess
{
    public class MisDBContext : DbContext
    {
        public MisDBContext(DbContextOptions<MisDBContext> options) : base(options)
        {
        }
        public DbSet<DistributorViewModel> DistributorList { get; set; }
        public DbSet<BTSViewModel> BTSList { get; set; }
        public DbSet<BackboneRouterSwitchViewModel> BackboneRouterSwitchList { get; set; }
        public DbSet<BackboneRouterSwitchInformationViewModel> BackboneRouterSwitchInformation { get; set; }
        public DbSet<PackagePlanViewModel> PackagePlanList { get; set; }
        public DbSet<PoolNameListModel> PoolNameList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
