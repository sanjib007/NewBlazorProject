using L3T.Infrastructure.Helpers.Models.SelfCare.Wfa2_133_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.SelfCareDbContext
{
    public class Wfa2_133DBContext : DbContext
    {
        public Wfa2_133DBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<VasProductsModel> VasProducts { get; set; }
    }
}
