using L3T.Infrastructure.Helpers.Models.Location;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.LocationDBContext
{
    public class LocationDataReadContext : DbContext
    {
        public LocationDataReadContext(DbContextOptions<LocationDataReadContext> options) : base(options)
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
