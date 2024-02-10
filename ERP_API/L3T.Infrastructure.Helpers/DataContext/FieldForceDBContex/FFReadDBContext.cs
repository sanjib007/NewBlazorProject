using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex
{
    public class FFReadDBContext : DbContext
    {
        public FFReadDBContext(DbContextOptions<FFReadDBContext> options) : base(options)
        {
        }
    }
}
