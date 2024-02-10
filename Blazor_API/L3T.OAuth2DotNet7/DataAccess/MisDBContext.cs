using L3T.OAuth2DotNet7.DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace L3T.OAuth2DotNet7.DataAccess
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
