using L3T.Infrastructure.Helpers.Models.Schedule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.ScheduleDBContext
{
	public class ScheduleDataContext : DbContext
	{
		public ScheduleDataContext(DbContextOptions<ScheduleDataContext> options) : base(options)
		{
		}
		public DbSet<Test> Test { get; set; }
		public DbSet<NetworkInformation> NetworkInformation { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//modelBuilder.Entity<BdIP>().HasNoKey().ToView("v_BdIP");
			//modelBuilder.Entity<SPOutput>().HasNoKey().ToView("v_SPOutput");
		}
	}
}

// Add-Migration init -Context L3T.Infrastructure.Helpers.DataContext.ScheduleDBContext.ScheduleDataContext
// Update - Database - Context L3T.Infrastructure.Helpers.DataContext.ScheduleDBContext.ScheduleDataContext
