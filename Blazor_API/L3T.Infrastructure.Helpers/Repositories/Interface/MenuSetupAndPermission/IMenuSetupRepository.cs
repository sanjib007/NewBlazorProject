using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.Parmission;
using L3T.Infrastructure.Helpers.Models.RequestModel.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Interface.MenuSetupAndPermission
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
        Task AutoUpdateCrApprovedToInProgress();





	}
}
