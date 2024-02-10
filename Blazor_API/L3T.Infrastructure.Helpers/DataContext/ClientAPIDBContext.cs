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
    public class ClientAPIDBContext : DbContext
    {
        public ClientAPIDBContext(DbContextOptions<ClientAPIDBContext> options) : base(options)
        {
        }
        
        public DbSet<ClientRequestResponseModel> RequestResponseLog { get; set; }
        //public DbSet<TblSlider> TblSlider { get; set; }
        //public DbSet<TblPackage> TblPackage { get; set; }
        //public DbSet<ProfileImage> ProfileImage { get; set; }
        //public DbSet<NotificationModel> Notifications { get; set; }
        //public DbSet<FirebaseRegisteredTokenModel> FirebaseRegisteredToken { get; set; }
        //public DbSet<FirebaseTopicCampaignDataModel> FirebaseTopicCampaignData { get; set; }


    }
}



// Add-Migration RequestResponseLog -Context L3T.Infrastructure.Helpers.DataContext.ClientAPIDBContext
// Update-Database -Context L3T.Infrastructure.Helpers.DataContext.ClientAPIDBContext