using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;
using Cr.UI.Data.CrStatus;
using Cr.UI.Data.StateManagement;
using Cr.UI.Services.Implementation;
using Cr.UI.Services.Interface;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cr.UI.ServiceDependencies
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services) {

            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvidore>();

            services.AddHttpClient<IUserService, UserService>();
            services.AddHttpClient<IDomainService, DomainService>();
            services.AddHttpClient<ICrReportService, CrReportService>();
            services.AddHttpClient<IChangeRequirementService, ChangeRequirementService>();
            services.AddHttpClient<INotificationService, NotificationService>();
            services.AddHttpClient<IMenuAndPermissionService, MenuAndPermissionService>(); 
            services.AddHttpClient<IApprovalFlowSetupService, ApprovalFlowSetupService>(); 




			services.AddHttpClient<IGenericService<TempChangeRequestedInfo>, GenericService<TempChangeRequestedInfo>>(); 
            services.AddHttpClient<IGenericService<StatusWiseTotalCrResponse>, GenericService<StatusWiseTotalCrResponse>>();
            services.AddHttpClient<IGenericService<CrApprovalFlow>, GenericService<CrApprovalFlow>>(); 
            services.AddHttpClient<IGenericService<DeveloperInformation>, GenericService<DeveloperInformation>>();
            services.AddHttpClient<IGenericService<CrDashboardResponseModel>, GenericService<CrDashboardResponseModel>>();
            

            services.AddScoped<AppState>();
            services.AddScoped<SpinnerState>();
            services.AddScoped<BlazorDisplaySpinnerAutomaticallyHttpMessageHandler>();
            services.AddSingleton<HttpClient>();

            return services;
        }
    }
}
