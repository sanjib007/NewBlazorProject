using L3T.Infrastructure.Helpers.Models;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext
{
    public class MysqlDBContext : DbContext
    {
        public MysqlDBContext(DbContextOptions<MysqlDBContext> options) : base(options)
        {

        }

       // public  DbSet<SelfcareRewardPointModel> referral_history_menual { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SelfcareRewardPointModel>().HasNoKey().ToView("v_SelfcareRewardPointModel");


            base.OnModelCreating(modelBuilder);
        }
    }
}
