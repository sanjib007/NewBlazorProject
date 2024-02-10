using L3T.Infrastructure.Helpers.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L3T.Infrastructure.Helpers.Models.Test;
using L3T.Infrastructure.Helpers.Models.TicketEntity;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class TicketDataWriteContext : DbContext
    {
        public TicketDataWriteContext(DbContextOptions<TicketDataWriteContext> options) : base(options)
        {
        }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TicketEntry> TicketEntry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
// EntityFrameworkCore\Add-Migration init -Context TicketDataContext 
// EntityFrameworkCore\Update-Database -Context TicketDataContext
