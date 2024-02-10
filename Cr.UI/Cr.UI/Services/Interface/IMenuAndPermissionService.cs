using Cr.UI.Data.Permission;
using Cr.UI.Data;

namespace Cr.UI.Services.Interface
{
    public interface IMenuAndPermissionService
    {
        Task<ApiResponse<PermissionAndSetupModel>> GetAllMenuWithPermission(string requestUri);
        Task<IDictionary<string, bool>> CheckPermission(string name);
        Task<ApiResponse<List<MenuSetupModel>>> GetAllMenuAndPermission(string requestUri);
        Task<ApiResponse> MenuUpdate(MenuUpdateRequestModel obj);
        Task<ApiResponse> PermissionUpdate(PermissionUpdateRequestModel obj);
        Task<ApiResponse> AnonymousPermissionUpdate(AnonymousPermissionRequestModel obj);
        Task<ApiResponse<List<string>>> GetAllRolesName(string requestUri);
        Task<List<string>> GetAllUsersBySearch(string requestUri);
        Task<ApiResponse<List<MenuSetupModel>>> SpecificUserOrRoleWisePermission(string requestUri, GetAllMenuSetupAndPermissionRequestModel obj, List<MenuSetupModel> allMenuPermission);
        Task<ApiResponse> UpdateAndDeleteMenuAndPermission(string requestUri, SetPermissionForRoleOrUserRequestModel obj);








	}
}
