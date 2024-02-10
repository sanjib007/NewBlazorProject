using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.Models.BTS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L3T.Infrastructure.Helpers.DataContext.BtsDBContext
{
    public class BtsDataReadContext : DbContext
    {
        public BtsDataReadContext(DbContextOptions<BtsDataReadContext> options) : base(options)
        {
        }

        public DbSet<BtsInfo> BtsInfo { get; set; }
        public DbSet<SupportOffice> SupportOffice { get; set; }
        public DbSet<BTSType> BTSType { get; set; }
        public DbSet<BTSMode> BTSMode { get; set; }
        public DbSet<TowerType> TowerType { get; set; }
        public DbSet<UPSBackup> UPSBackup { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Router> Routers { get; set; }
        public DbSet<RouterType> RouterTypes { get; set; }
        public DbSet<Switch> Switch { get; set; }
        public DbSet<SwitchType> SwitchTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}


// Add-Migration init -Context L3T.Infrastructure.Helpers.DataContext.BtsDBContext.BtsDataReadContext
// Update - Database - Context L3T.Infrastructure.Helpers.DataContext.BtsDBContext.BtsDataReadContext