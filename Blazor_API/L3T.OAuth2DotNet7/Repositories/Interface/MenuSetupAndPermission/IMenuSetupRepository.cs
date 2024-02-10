using L3T.OAuth2DotNet7.DataAccess.Model.Parmission;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission.RequestModel;

namespace L3T.OAuth2DotNet7.Repositories.Interface.MenuSetupAndPermission
{
    public interface IMenuSetupRepository
    {
        Task InsertMenuSetupTable(string controllerName, string actionName, string url, string projectName, string type);
        Task<List<MenuSetupAndPermissionViewModel>> getAllMenuSetupAndRoleWiseMenuPermissionProjectWise(string projectName);
        Task<List<MenuSetupAndPermissionViewModel>> getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise(GetAllMenuSetupAndPermissionRequestModel model);
        Task<List<MenuSetupModel>> getMenuAndPermissionSetup(string projectName);
        Task<bool> SingleMenuUpdate(MenuSetupRequestModel model);
        Task<bool> PermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model);
        Task<bool> DeletePermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model);
        Task<List<MenuSetupAndPermissionViewModel>> PermissionCheckFromMiddleware(GetAllMenuSetupAndPermissionRequestModel model, string controllerName, string methodName);





    }
}
