using L3T.Infrastructure.Helpers.Models.SmsNotification;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext;

public class SMSNotifyReadDBContext : DbContext
{
    public SMSNotifyReadDBContext(DbContextOptions<SMSNotifyReadDBContext> options) : base(options)
    {
    }
    
    public DbSet<raw_data> raw_data { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}