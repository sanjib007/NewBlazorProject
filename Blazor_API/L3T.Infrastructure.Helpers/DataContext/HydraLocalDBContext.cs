using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.ResponseModel;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.Hydra;
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
    public class HydraLocalDBContext : DbContext
    {
        public HydraLocalDBContext(DbContextOptions<HydraLocalDBContext> options) : base(options)
        {
        }
       
        public  DbSet<NetworkInformationResponseModel> NetworkInformationResponseModel { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Mis_Profile_View>().HasNoKey().ToView("v_Mis_Profile_View");
            //modelBuilder.Entity<Tbl_complain_info>().HasNoKey().ToView("Tbl_complain_info");

            base.OnModelCreating(modelBuilder);
          
        }
    }
}
