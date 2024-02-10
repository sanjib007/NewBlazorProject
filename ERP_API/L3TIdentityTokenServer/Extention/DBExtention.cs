using L3T.Infrastructure.Helpers.DataContext;
using L3TIdentityTokenServer.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace L3TIdentityTokenServer.Extention
{
    public static class DBExtention
    {
        public static IServiceCollection AddDBExtention(this IServiceCollection services, ConfigurationManager Configuration)
        {
            // Replace with your connection string.
//            var connectionString = Configuration["ConnectionStrings:Default"];
            
            services.AddDbContext<IdentityTokenServerDBContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("IdentityConnection");
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(connectionString);

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });
            
            return services;
        }
        
    }
}