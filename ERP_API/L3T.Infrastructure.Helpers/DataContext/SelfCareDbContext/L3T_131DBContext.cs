using L3T.Infrastructure.Helpers.Models.SelfCare.L3T_131_Model;
using L3T.Infrastructure.Helpers.Models.SelfCare.Wfa2_133_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.SelfCareDbContext
{
    public class L3T_131DBContext : DbContext
    {
        public L3T_131DBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<slsItemDetailsModel> GetProductInfo { get; set; }
        public DbSet<RevenueSalesModel> GetRevenueSalesInfo { get; set; }
        public DbSet<ClientMainModel> GetCustomerInfo { get; set; }
        public DbSet<MaxSalesIDModel> GetMaxSalesID { get; set; }
        public DbSet<ServiceCreateSPModel> CreateServiceSP { get; set; }

    }
}
