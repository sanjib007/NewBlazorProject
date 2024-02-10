using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class RsmDBContext : DbContext
    {
        public RsmDBContext(DbContextOptions<RsmDBContext> options) : base(options)
        {
        }
        public DbSet<RsmProfileResModel> Rsm_Profile_View { get; set; }

        public virtual DbSet<RSM_Complain_Details> RSM_Complain_Details { get; set; }
        public virtual DbSet<RSM_ComplainLogDetails> RSM_ComplainLogDetails { get; set; }
        public virtual DbSet<tbl_user_info> tbl_user_info { get; set; }
        public virtual DbSet<RSM_NatureSetup> RSM_NatureSetup { get; set; }
        public virtual DbSet<tbl_Team_info> tbl_Team_info { get; set; }
        public virtual DbSet<RSM_TaskPendingTeam> RSM_TaskPendingTeam { get; set; }
        public virtual  DbSet<RSM_clientDatabaseMain> ClientDatabaseMain { get; set; }
        public DbSet<RSM_ComplainLogDetailsModel> RSM_ComplainLogDetailsModel { get; set; }
        public DbSet<RsmCustomerInfoResModel> RsmCustomerInfoResModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Rsm_Profile_View>().HasNoKey().ToView("v_Rsm_Profile_View");
            //modelBuilder.Entity<RSM_Complain_Details>().HasNoKey().ToView("v_RSM_Complain_Details");
            //modelBuilder.Entity<RSM_ComplainLogDetails>().HasNoKey().ToView("v_RSM_ComplainLogDetails");
            //modelBuilder.Entity<tbl_user_info>().HasNoKey().ToView("v_tbl_user_info");

            base.OnModelCreating(modelBuilder);
        }
    }
}
