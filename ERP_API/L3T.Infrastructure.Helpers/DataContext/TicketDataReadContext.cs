using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L3T.Infrastructure.Helpers.Models.TicketEntity;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class TicketDataReadContext : DbContext
    {
        public TicketDataReadContext(DbContextOptions<TicketDataReadContext> options) : base(options)
        {
        }
        public DbSet<TicketEntry> TicketEntry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
