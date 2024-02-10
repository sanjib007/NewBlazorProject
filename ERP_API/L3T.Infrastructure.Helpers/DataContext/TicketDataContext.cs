using L3T.Infrastructure.Helpers.Models.TicketEntity;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class TicketDataContext : DbContext
    {
        public TicketDataContext(DbContextOptions<TicketDataContext> options) : base(options)
        {
        }

        public DbSet<TicketEntry> TicketEntry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
// EntityFrameworkCore\Add-Migration init -Context TicketDataContext 
// EntityFrameworkCore\Update-Database -Context TicketDataContext
