using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.IpServiceDBContext;

public class IpServiceDataReadContext : DbContext
{
    public IpServiceDataReadContext(DbContextOptions<IpServiceDataReadContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}