using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex
{
    public class HydraDBContext : DbContext
    {
        public HydraDBContext(DbContextOptions<HydraDBContext> options) : base(options)
        {
        }
        public DbSet<HydraBalanceInfoModel> HydraBalanceInfo { get; set; }
        public DbSet<HydraPackageInformationModel> HydraPackageInformation { get; set; }


    }
}
