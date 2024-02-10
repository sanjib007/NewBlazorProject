using L3TIdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace L3TIdentityServer.DataAccess
{
    public class L3TIdentityContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public L3TIdentityContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        //public new DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
