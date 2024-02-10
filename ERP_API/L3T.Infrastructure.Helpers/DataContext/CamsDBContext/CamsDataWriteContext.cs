using L3T.Infrastructure.Helpers.Models.Link3Subscriber;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel;
using L3T.Infrastructure.Helpers.Models.SystemErrorLog;
using L3T.Infrastructure.Helpers.Models.SystemSetting;
using L3T.Infrastructure.Helpers.Models.Test;
using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.DataContext.CamsDBContext;

public class CamsDataWriteContext : DbContext
{
    public CamsDataWriteContext(DbContextOptions<CamsDataWriteContext> options) : base(options)
    {
    }
    public DbSet<Test> Tests { get; set; }
    
    public DbSet<Link3Subscriber> Link3Subscribers { get; set; }
    public DbSet<SystemErrorLog> SystemErrorLogs { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }
    public DbSet<MikrotikRequestResponse> MikrotikRequestResponses { get; set; }
    public DbSet<GetMikrotikRouterUserInfoResponseModel> MikrotikRouterUserInfos { get; set; }
    public DbSet<GetMikrotikRouterInterfaceModel> MikrotikRouterInterface { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

//dotnet ef migrations add "InitialLink3SubcribersTable"  -p "L3T.Infrastructure.Helpers" -c  CamsDataWriteContext -s "L3T.CAMS"
//dotnet ef database update  -p "L3T.Infrastructure.Helpers" -c  CamsDataWriteContext -s "L3T.CAMS"