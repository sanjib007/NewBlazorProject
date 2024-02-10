using L3TIdentityTokenServer.Services.Implementation;
using L3TIdentityTokenServer.Services.Interface;

namespace L3TIdentityTokenServer.Services;

public static class ServiceDependency
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        return services;
    }
    
}