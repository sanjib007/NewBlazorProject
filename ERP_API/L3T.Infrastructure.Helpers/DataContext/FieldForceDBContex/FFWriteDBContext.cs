using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex
{
    public class FFWriteDBContext : DbContext
    {
        public FFWriteDBContext(DbContextOptions<FFWriteDBContext> options) : base(options)
        {
        }
        public DbSet<FFRequestResponseModel> fFRequestResponseModels { get; set; }
        public DbSet<LatLonModel> LatLon { get; set; }
    }
}
//dotnet ef migrations add "FieldForceInit"  -p "L3T.Infrastructure.Helpers" -c  FFWriteDBContext -s "L3T.FieldForceApi"
//dotnet ef database update  -p "L3T.Infrastructure.Helpers" -c  FFWriteDBContext -s "L3T.FieldForceApi"