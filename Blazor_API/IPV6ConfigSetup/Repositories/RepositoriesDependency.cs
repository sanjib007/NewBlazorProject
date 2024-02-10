using IPV6ConfigSetup.Repositories.Implementation;
using IPV6ConfigSetup.Repositories.Interface;

namespace IPV6ConfigSetup.Repositories
{
    public static class RepositoriesDependency
    {
        public static IServiceCollection AddRepositoryDependency(this IServiceCollection services)
        {
            services.AddTransient<IIPVSetupRepository, IPVSetupRepository>();
            services.AddTransient<IMisDBRepository, MisDBRepository>();
            return services;
        }
    }
}
