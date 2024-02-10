using L3T.Infrastructure.Helpers.Services.ServiceImplementation.Email;
using L3T.Infrastructure.Helpers.Services.ServiceImplementation.MenuSetupAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceImplementation.ThirdParty;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Email;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.ThirdParty;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.Services
{
	public static class ServicesDependency
    {
        public static IServiceCollection AddServicesDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(ITempChangeRequestedInfoService), typeof(TempChangeRequestedInfoService));
            services.AddScoped(typeof(IChangeRequestedInfoService), typeof(ChangeRequestedInfoService));
            services.AddScoped(typeof(IAssignEmployeeService), typeof(AssignEmployeeService));
            services.AddScoped(typeof(ICrStatusService), typeof(CrStatusService));
            services.AddScoped(typeof(ICrApprovalFlowService), typeof(CrApprovalFlowService));
            services.AddScoped(typeof(ICrDefaultApprovalFlowService), typeof(CrDefaultApprovalFlowService));
            services.AddScoped(typeof(INotificationDetailsService), typeof(NotificationDetailsService));
            services.AddScoped(typeof(ICRRequestResponseService), typeof(CRRequestResponseService));
            services.AddScoped(typeof(IMailSenderService), typeof(MailSenderService));
            services.AddScoped(typeof(IBaseControllerCommonService), typeof(BaseControllerCommonService));
            services.AddScoped(typeof(IMenuAndPermissionSetupService), typeof(MenuAndPermissionSetupService));
			services.AddScoped(typeof(IThirdPartyService), typeof(ThirdPartyService));
            services.AddScoped(typeof(IChangeRequestLogService), typeof(ChangeRequestLogService));

            return services;
        }
    }
}
