using L3TIdentityOAuth2Server.Services.Implementation;
using L3TIdentityOAuth2Server.Services.Interface;

namespace L3TIdentityOAuth2Server.Services;

public static class ServiceDependency
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IThirdPartyHttpRequestService, ThirdPartyRequestService>();
        services.AddTransient<IThirdPartyHttpRequestService, ThirdPartyRequestService>();
        services.AddTransient<INotificationSendService, NotificationSendService>();
        services.AddTransient<IPushNotificationService, PushNotificationService>();
        services.AddTransient<IIdentityRequestResponseService, IdentityRequestResponseService>();
        return services;
    }

}