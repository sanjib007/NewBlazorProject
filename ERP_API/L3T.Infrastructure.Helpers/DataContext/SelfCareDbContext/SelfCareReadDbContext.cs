using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.SelfCareDbContext
{
    public class SelfCareReadDbContext : DbContext
    {
        public SelfCareReadDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
