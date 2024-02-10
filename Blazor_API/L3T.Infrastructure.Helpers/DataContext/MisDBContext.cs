using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.ResponseModel;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS;
using L3T.Infrastructure.Helpers.Repositories.Implementation.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class MisDBContext : DbContext
    {
        public MisDBContext(DbContextOptions<MisDBContext> options) : base(options)
        {
        }
       
        public virtual DbSet<MisCustomerInfoResModel> MisCustomerInfoResModel { get; set; }

        public virtual DbSet<TicketListResponseModel> TicketListResponseModel { get; set; }

        //public virtual DbSet<Mis_Profile_View> Mis_Profile_View { get; set; }
        public virtual DbSet<Tbl_complain_info> Tbl_complain_info { get; set; }
        public virtual DbSet<Tbl_Com_Category> Tbl_Com_Category { get; set; }
        public virtual DbSet<ClientDatabaseMain> ClientDatabaseMain { get; set; }
        public virtual DbSet<T_Client> T_Client { get; set; }
        public virtual DbSet<T_Project> T_Project { get; set; }
        public virtual DbSet<Tbl_client_com_det> Tbl_client_com_det { get; set; }
        public virtual DbSet<Tbl_ComplainAccessPermission> Tbl_ComplainAccessPermission { get; set; }
        public virtual DbSet<Tbl_Ticket_ForwardHistory> Tbl_Ticket_ForwardHistory { get; set; }
        public virtual DbSet<TblComplainEmailFormat> TblComplainEmailFormat { get; set; }
        public virtual DbSet<TmpTID> TmpTID { get; set; }
        public virtual DbSet<Max_com_Ticketref> Max_com_Ticketref { get; set; }
        public DbSet<MisNetworkInformationResponseModel> MisNetworkInformationResponseModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Mis_Profile_View>().HasNoKey().ToView("v_Mis_Profile_View");
            //modelBuilder.Entity<Tbl_complain_info>().HasNoKey().ToView("Tbl_complain_info");

            base.OnModelCreating(modelBuilder);
          
        }
    }
}
