using L3T.Infrastructure.Helpers.Services.ServiceImplementation;
using L3T.Infrastructure.Helpers.Services.ServiceImplementation.Client;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Client;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.Services
{
    public static class ServicesClientDependency
    {
        public static IServiceCollection AddServicesClientDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(IMISCustomerInformationService), typeof(MISCustomerInformationService));
            services.AddScoped(typeof(ITrackingIssueService), typeof(TrackingIssueService));
            services.AddScoped(typeof(IClientReqResService), typeof(ClientReqResService));

            return services;
        }
    }
}
