using L3T.Infrastructure.Helpers.Repositories.Implementation.Client;
using L3T.Infrastructure.Helpers.Repositories.Interface.Client;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.Repositories
{
    public static class RepositoriesClientDependency
    {
        public static IServiceCollection AddRepositoriesClientDependency(this IServiceCollection services)
        {

            services.AddScoped(typeof(IMISCustomerInformationRepository), typeof(MISCustomerInformationRepository));
            services.AddScoped(typeof(ITrackingIssueRepository), typeof(TrackingIssueRepository));

            return services;
        }
    }
}
