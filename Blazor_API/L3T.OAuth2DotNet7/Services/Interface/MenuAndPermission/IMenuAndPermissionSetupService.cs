using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission.RequestModel;

namespace L3T.OAuth2DotNet7.Services.Interface.MenuAndPermission
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
