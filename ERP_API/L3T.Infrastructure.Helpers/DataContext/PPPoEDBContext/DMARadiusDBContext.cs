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
    public class DMARadiusDBContext : DbContext
    {
        public DMARadiusDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<radcheck> radcheck { get; set; }
        public DbSet<rm_services> rm_services { get; set; }
        public DbSet<rm_changesrv> rm_changesrv { get; set; }
        public DbSet<rm_users> rm_users { get; set; }
        public DbSet<nas> nas { get; set; }
        public DbSet<rm_ippools> rm_ippools { get; set; }
        public DbSet<radippool> radippool { get; set; }
    }
}
