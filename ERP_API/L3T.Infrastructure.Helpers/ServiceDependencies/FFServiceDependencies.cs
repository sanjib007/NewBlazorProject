using L3T.Infrastructure.Helpers.Implementation.Email;
using L3T.Infrastructure.Helpers.Implementation.FieldForce;
using L3T.Infrastructure.Helpers.Implementation.ThirdParty;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Interface.ThirdParty;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class FFServiceDependencies
    {
        public static IServiceCollection AddFFServiceDependecy(this IServiceCollection services)
        {
            services.AddScoped(typeof(IFieldForceService), typeof(FieldForceService));
            services.AddScoped(typeof(IForwardTicketService), typeof(ForwardTicketService));
            services.AddScoped(typeof(IMailSenderService), typeof(MailSenderService));
            services.AddScoped(typeof(IInstallationTicketService), typeof(InstallationTicketService));
            services.AddScoped(typeof(IInstallationTicketFileUploadService), typeof(InstallationTicketFileUploadService));
            services.AddScoped(typeof(IChecklistService), typeof(ChecklistService));
            services.AddScoped(typeof(IRsmInstallationTicketService), typeof(RsmInstallationTicketService));
            services.AddScoped(typeof(IRSMComplainTicketService), typeof(RSMComplainTicketService)); 
            services.AddScoped(typeof(IRsmChecklistService), typeof(RsmChecklistService));
            services.AddScoped(typeof(IOtherService), typeof(OtherService));


            return services;
        }
    }
}
