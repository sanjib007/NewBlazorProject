using L3T.Infrastructure.Helpers.Implementation.Cams;
using L3T.Infrastructure.Helpers.Implementation.Common;
using L3T.Infrastructure.Helpers.Implementation.Email;
using L3T.Infrastructure.Helpers.Implementation.IP;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Interface.Common;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.IP;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.ServiceDependencies;

public static class CAMSServiceDependencyExtention
{
    public static IServiceCollection AddCAMSServiceDependecy(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICamsService), typeof(CamsService));
        services.AddScoped(typeof(IMailSenderService), typeof(MailSenderService));
        services.AddScoped(typeof(ISystemService), typeof(SystemService));
        services.AddScoped(typeof(IIPWhiteListedService), typeof(IPWhiteListedService));
        services.AddScoped(typeof(ISocketService), typeof(SocketService)); 
        return services;
    }
}