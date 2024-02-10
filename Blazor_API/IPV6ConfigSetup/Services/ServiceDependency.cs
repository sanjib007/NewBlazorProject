using IPV6ConfigSetup.Services.Implementation;
using IPV6ConfigSetup.Services.Interface;

namespace IPV6ConfigSetup.Services;

public static class ServiceDependency
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddTransient<IIPVSetupService, IPVSetupService>();
        services.AddTransient<IMisDBService, MisDBService>(); 


        return services;
    }

}