using L3T.Infrastructure.Helpers.Implementation.Common;
using L3T.Infrastructure.Helpers.Implementation.Email;
using L3T.Infrastructure.Helpers.Implementation.SMSNotify;
using L3T.Infrastructure.Helpers.Interface.Common;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.SMSNotify;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.ServiceDependencies;

public static class SMSNotifyServiceDependencyExtention
{
    public static IServiceCollection AddSMSNotifyServiceDependecy(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISMSNotifyService), typeof(SMSNotifyService));
        services.AddScoped(typeof(IMailSenderService), typeof(MailSenderService));
        return services;
    }
}