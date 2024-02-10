using L3T.Infrastructure.Helpers.Models.Mikrotik;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.IPWhiteListDBContext
{
    public class IpWhiteListedDBContext : DbContext
    {
        public IpWhiteListedDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<WhiteListedIp> WhiteListedIPs { get; set; }
    }
}


//dotnet ef migrations add "InitialLink3SubcribersTable"  -p "L3T.Infrastructure.Helpers" -c  IpWhiteListedDBContext -s "L3T.CAMS"
//dotnet ef database update  -p "L3T.Infrastructure.Helpers" -c  IpWhiteListedDBContext -s "L3T.CAMS"