using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;

namespace L3TIdentityOAuth2Server.Services.Interface;

public interface IAccountService
{
    Task<ApiSuccess> ChangePasswordAsync(ChangePasswordRequestModel request, string userId);
    Task<ApiSuccess> GenerateForgetPasswordTokenAsync(ForgetPasswordRequestModel request);
    Task<ApiSuccess> ResetPasswordAsync(ResetPasswordModel requestModel);
    Task<ApiSuccess> ChangePhoneNumberAsync(ChangePhoneNumberRequestModel requestModel, string userId);
    Task<ApiSuccess> ConfirmPhoneNumberAsync(ConfirmPhoneNumberRequestModel requestModel, string userId);
    Task<ApiSuccess> GeneratePhoneNumberTokenAsync(ChangePhoneNumberRequestModel requestModel, string userId);
    Task<ApiSuccess> ChangeEmailAsync(ChangeEmailRequestModel requestModel, string userId);
    Task<ApiSuccess> ChangeEmailConfirmWithToken(ConfirmEmailRequestModel requestModel, string userId);
    Task<ApiSuccess> GetProfile(string userId);
    Task<ApiSuccess> GetRolesByUserIdAsync(string userId);
    Task<ApiSuccess> AddUserToRoleAsync(string role, string userId);
    Task<ApiSuccess> RemoveUserFromRoleAsync(string role, string userId);
    Task<ApiSuccess> AddUserToRolesAsync(AddUserToRolesRequest request, string requestUserName);
    Task<ApiSuccess> RemoveUserFromRolesAsync(AddUserToRolesRequest request, string requestUserName);
    Task<ApiSuccess> GetUserClaimsAsync(GetUserClaimsRequestModel request);
    Task<ApiSuccess> GetUserRolesAsync(GetUserRolesRequestModel request);
    Task<ApiSuccess> GetAllUsersAsync(GetUserWithFilter model, string route);
    Task<ApiSuccess> GetAllRolesAsync();
    Task<ApiSuccess> GetAllUserClaimsAsync();
    Task<ApiSuccess> GetAllRoleClaimsAsync();
    Task<ApiSuccess> EditRole(EditRoleNameRequestModel req);
    Task<ApiSuccess> DeleteRole(string id);
    Task<ApiSuccess> UpdateUserInfo(string id, UpdateUserRequestModel model, string userId);
    Task<ApiSuccess> DeleteUserInfo(string id);
    Task<ApiSuccess> AddUserInfo(UserRequestModel model, string userId);

}