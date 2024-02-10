using Elasticsearch.Net;
using L3T.OAuth2DotNet7.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace L3T.OAuth2DotNet7.Extention
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
            services.AddDbContextPool<MisDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("MisConnection")); //I set this on appsettings.json
            });

            services.AddDbContextPool<LNKDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("LNKDB")); // Set this on appsettings.json
            });
            services.AddDbContextPool<ApplicationMenuAndRoleWiseMenuPermissionDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("PermissionDB")); // Set this on appsettings.json
            });

            return services;
        }

    }
}