using L3T.Infrastructure.Helpers.Models.Ipservice.Entities;
using L3T.Infrastructure.Helpers.Models.Link3Subscriber;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel;
using L3T.Infrastructure.Helpers.Models.SystemErrorLog;
using L3T.Infrastructure.Helpers.Models.SystemSetting;
using L3T.Infrastructure.Helpers.Models.Test;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.IpServiceDBContext;

public class IpServiceDataWriteContext : DbContext
{
    public IpServiceDataWriteContext(DbContextOptions<IpServiceDataWriteContext> options) : base(options)
    {
    }

    public DbSet<GatewayIpAddress> GatewayIpAddresses { get; set; }

    public DbSet<GatewayWiseClientIpAddress> GatewayWiseClientIpAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

//dotnet ef migrations add "InitialLink3GatewayIpAndClientIp"  -p "L3T.Infrastructure.Helpers" -c  IpServiceDataWriteContext -s "L3T.IPService"
//dotnet ef database update  -p "L3T.Infrastructure.Helpers" -c  IpServiceDataWriteContext -s "L3T.IPService"