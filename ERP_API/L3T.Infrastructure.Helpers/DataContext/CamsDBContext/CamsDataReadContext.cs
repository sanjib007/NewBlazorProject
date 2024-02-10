using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.CamsDBContext;

public class CamsDataReadContext : DbContext
{
    public CamsDataReadContext(DbContextOptions<CamsDataReadContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}