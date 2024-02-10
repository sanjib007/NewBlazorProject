using L3T.Infrastructure.Helpers.Models.Location;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.LocationDBContext
{
    public class LocationDataWriteContext : DbContext
    {
        public LocationDataWriteContext(DbContextOptions<LocationDataWriteContext> options) : base(options)
        {
        }

        public DbSet<Zone> Zone { get; set; }
        public DbSet<Division> Division { get; set; }

        public DbSet<District> District { get; set; }

        public DbSet<Upazila> Upazila { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}


// Add-Migration init -Context L3T.Infrastructure.Helpers.DataContext.LocationDBContext.LocationDataWriteContext
// Update-Database -Context L3T.Infrastructure.Helpers.DataContext.LocationDBContext.LocationDataWriteContext
