using L3T.Infrastructure.Helpers.DataContext;
using Microsoft.EntityFrameworkCore;

namespace L3T.Client.API.Extention
{
    public static class DBExtension
    {
        public static IServiceCollection AddDBExtention(this IServiceCollection services, ConfigurationManager Configuration)
        {
            
            services.AddDbContextPool<MisDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("MisConnection")); //I set this on appsettings.json
            });

            services.AddDbContextPool<RsmDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("RsmConnString")); //I set this on appsettings.json
            });

           
            services.AddDbContextPool<ClientAPIDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("ClientApiString")); //I set this on appsettings.json
            });
            
            services.AddDbContextPool<HydraLocalDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("HydraLocalDBString")); //I set this on appsettings.json
            });



            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            //services.AddDbContext<MysqlDBContext>(
            //    dbContextOptions => dbContextOptions
            //        .UseMySql(Configuration.GetConnectionString("mySQLConnection"), serverVersion)
            //        .LogTo(Console.WriteLine, LogLevel.Information)
            //        .EnableSensitiveDataLogging()
            //        .EnableDetailedErrors()
            //);


            return services;
        }
    }
}
