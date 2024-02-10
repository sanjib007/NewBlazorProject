using L3TIdentityOAuth2Server.DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace L3TIdentityOAuth2Server.DataAccess
{
    public class MisDBContext : DbContext
    {
        public MisDBContext(DbContextOptions<MisDBContext> options) : base(options)
        {
        }
        public DbSet<UserProfileInformation> UserProfileInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
