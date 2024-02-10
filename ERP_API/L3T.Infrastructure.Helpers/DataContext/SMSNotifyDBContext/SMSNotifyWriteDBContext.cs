using L3T.Infrastructure.Helpers.Models.SmsNotification;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext;

public class SMSNotifyWriteDBContext : DbContext
{
    public SMSNotifyWriteDBContext(DbContextOptions<SMSNotifyWriteDBContext> options) : base(options)
    {
    }
    
    public DbSet<SmsNotification> SmsNotification { get; set; }
    public DbSet<SmsNotifyRequestResponse> SmsNotifyRequestResponse { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

//dotnet ef migrations add "InitialSmsNotify"  -p "L3T.Infrastructure.Helpers" -c  SMSNotifyWriteDBContext -s "L3T.SMSNotify"
//dotnet ef database update  -p "L3T.Infrastructure.Helpers" -c  SMSNotifyWriteDBContext -s "L3T.SMSNotify"