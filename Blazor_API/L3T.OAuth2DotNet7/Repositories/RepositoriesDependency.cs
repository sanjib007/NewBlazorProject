using L3T.OAuth2DotNet7.Repositories.Implementation.MenuSetupAndPermission;
using L3T.OAuth2DotNet7.Repositories.Interface.MenuSetupAndPermission;

namespace L3T.OAuth2DotNet7.Repositories
{
    public static class RepositoriesDependency
    {
        public static IServiceCollection AddRepositoryDependency(this IServiceCollection services)
        {
            services.AddTransient<IMenuSetupRepository, MenuSetupRepository>();
            return services;
        }
    }
}
