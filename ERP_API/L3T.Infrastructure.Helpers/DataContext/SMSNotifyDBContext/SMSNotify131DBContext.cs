using L3T.Infrastructure.Helpers.Models.SmsNotification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext
{
    public class SMSNotify131DBContext : DbContext
    {
        public SMSNotify131DBContext(DbContextOptions<SMSNotify131DBContext> options) : base(options)
        {
        }

        public DbSet<ClientDatabaseMain> ClientDatabaseMain { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
