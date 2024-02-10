using L3T.OAuth2DotNet7.Repositories.Implementation.MenuSetupAndPermission;
using L3T.OAuth2DotNet7.Repositories.Interface.MenuSetupAndPermission;
using L3T.OAuth2DotNet7.Services.Implementation;
using L3T.OAuth2DotNet7.Services.Implementation.MenuSetupAndPermission;
using L3T.OAuth2DotNet7.Services.Interface;
using L3T.OAuth2DotNet7.Services.Interface.MenuAndPermission;

namespace L3T.OAuth2DotNet7.Services;

public static class ServiceDependency
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IThirdPartyHttpRequestService, ThirdPartyRequestService>();
        services.AddTransient<IRSAEncryptionSerivce, RSAEncryptionSerivce>();
        services.AddTransient<IIdentityReauestResponseService, IdentityReauestResponseService>();
        services.AddTransient<IMenuAndPermissionSetupService, MenuAndPermissionSetupService>();
        return services;
    }



}