using L3T.Infrastructure.Helpers.Repositories.Implementation;
using L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Implementation.MenuSetupAndPermission;
using L3T.Infrastructure.Helpers.Repositories.Interface;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface.MenuSetupAndPermission;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.Repositories
{
    public static class RepositoriesDependency
    {
        public static IServiceCollection AddRepositoriesDependency(this IServiceCollection services)
        {

            services.AddScoped(typeof(ITempChangeRequestedInfoRepository), typeof(TempChangeRequestedInfoRepository));            
            services.AddScoped(typeof(IChangeRequestedInfoRepository), typeof(ChangeRequestedInfoRepository));            
            services.AddScoped(typeof(IAssignEmployeeRepository), typeof(AssignEmployeeRepository));
            services.AddScoped(typeof(IAssignDeveloperRepository), typeof(AssignDeveloperRepository));
            services.AddScoped(typeof(ICrStatusRepository), typeof(CrStatusRepository));            
            services.AddScoped(typeof(ICrApprovalFlowRepository), typeof(CrApprovalFlowRepository));            
            services.AddScoped(typeof(ICrDefaultApprovalFlowRepository), typeof(CrDefaultApprovalFlowRepository));
            services.AddScoped(typeof(INotificationDetailsRepository), typeof(NotificationDetailsRepository));
            services.AddScoped(typeof(ICRRequestResponseRepository), typeof(CRRequestResponseRepository));
            services.AddScoped(typeof(ICrAttatchedFilesRepository), typeof(CrAttatchedFilesRepository));
            services.AddScoped(typeof(IMenuSetupRepository), typeof(MenuSetupRepository));
            services.AddScoped(typeof(IChangeRequestLogRepository), typeof(ChangeRequestLogRepository));
            services.AddScoped(typeof(ICommonRepository), typeof(CommonRepository)); 

            return services;
        }
    }
}
