using IPV6ConfigSetup.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace IPV6ConfigSetup.Extention
{
    public static class DBExtention
    {
        public static IServiceCollection AddDBExtention(this IServiceCollection services, ConfigurationManager Configuration)
        {
            // Replace with your connection string.
            //            var connectionString = Configuration["ConnectionStrings:Default"];

            services.AddDbContext<IPV6ConfigSetupDBContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("IdentityConnection");
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(connectionString);
            });
            services.AddDbContextPool<MisDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("MisConnection")); //I set this on appsettings.json
            });

            return services;
        }

    }
}