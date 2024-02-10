using L3T.Infrastructure.Helpers.Models.SelfCare.L3T_131_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.SelfCareDbContext
{
    public class WFA2_131DBContext : DbContext
    {
        public WFA2_131DBContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<ClientMainModel> GetCustomerInfo { get; set; }
    }
}
