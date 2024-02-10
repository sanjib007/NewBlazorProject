using L3T.Infrastructure.Helpers.Models.OLT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.OltDBContext
{
    public class OltDataWriteContext : DbContext
    {
        public OltDataWriteContext(DbContextOptions<OltDataWriteContext> options) : base(options)
        {
        }

        public DbSet<OltInfo> OltInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

// Add-Migration init -Context L3T.Infrastructure.Helpers.DataContext.OltDBContext.OltDataWriteContext
// Update-Database - Context L3T.Infrastructure.Helpers.DataContext.OltDBContext.OltDataWriteContext