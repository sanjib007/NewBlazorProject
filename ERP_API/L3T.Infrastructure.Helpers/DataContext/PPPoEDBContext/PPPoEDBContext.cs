using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using L3T.Infrastructure.Helpers.Models.PPPoE;
using L3T.Infrastructure.Helpers.Models.SmsNotification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.PPPoEDBContext
{
    public class PPPoEDBContext : DbContext
    {
        public PPPoEDBContext(DbContextOptions<PPPoEDBContext> options) : base(options)
        {
        }

        public DbSet<PPPoERequestResponseModel> PPPoERequestResponse { get; set; }

        public DbSet<radcheck> radcheck { get; set; } 

    }
}
