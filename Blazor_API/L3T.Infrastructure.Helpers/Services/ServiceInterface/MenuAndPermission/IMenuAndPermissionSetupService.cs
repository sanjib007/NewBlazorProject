using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.Parmission;
using L3T.Infrastructure.Helpers.Models.RequestModel.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission
{
    public interface IMenuAndPermissionSetupService
    {
        Task<ApiResponse> SpecificUserAndRoleWiseGetAllMenuAndPermission(GetAllMenuSetupAndPermissionRequestModel model, string ip, string userId);
        Task<ApiResponse> getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise(GetAllMenuSetupAndPermissionRequestModel model, string ip, string userId);
        Task<ApiResponse> getMenuAndPermissionSetup(string projectName, string ip, string userId);
        Task<ApiResponse> SingleMenuUpdate(MenuSetupRequestModel model, string ip, string userId);
        Task<ApiResponse> PermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model, string ip, string userId);
        Task<ApiResponse> DeletePermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model, string ip, string userId);
        Task<List<MenuSetupAndPermissionViewModel>> PermissionCheckFromMiddleware(GetAllMenuSetupAndPermissionRequestModel model, string controllerName, string methodName, string ip, string userId);


    }
}
