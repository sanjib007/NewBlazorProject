using L3T.Infrastructure.Helpers.Models.SelfCare.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.DataContext.SelfCareDbContext
{
    public class SelfCareWriteDbContext : DbContext
    {
        public SelfCareWriteDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SCRequestResponseModel> SCRequestResponses { get; set; }




    }


    //dotnet ef migrations add "SCInitial"  -p "L3T.Infrastructure.Helpers" -c  SelfCareWriteDbContext -s "L3T.SelfcareAPI"
    //dotnet ef database update  -p "L3T.Infrastructure.Helpers" -c  SelfCareWriteDbContext -s "L3T.SelfcareAPI"
}
