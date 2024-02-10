using L3T.Utility.Helper;
using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.IdentityModels;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;
using L3TIdentityOAuth2Server.DataAccess.ViewModel;
using L3TIdentityOAuth2Server.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3TIdentityOAuth2Server.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
public class AccountController : Controller
{
    private readonly RoleManager<AppRoles> _roleManager;
    private readonly IAccountService _accountService;
    public AccountController(
        RoleManager<AppRoles> roleManager,
        IAccountService accountService)
    {
        _roleManager = roleManager;
        _accountService = accountService;
    }

    [HttpPost("RoleInsert")]
    public async Task<IActionResult> RoleInsert(InsertRoleRequestModel model)
    {
        var isHave = await _roleManager.RoleExistsAsync(model.RoleName);
        if (isHave)
        {
            throw new ApplicationException("This role is already exist.");
        }

        var newRole = new AppRoles()
        {
            Name = model.RoleName
        };
        await _roleManager.CreateAsync(newRole);
        return Ok(new ApiSuccess()
        {
            Status = "Success",
            StatusCode = 200,
            Message = "Role insert successful."
        });
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel request)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.ChangePasswordAsync(request, userId);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("ForgetPassword")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestModel requestModel)
    {
        var response = await _accountService.GenerateForgetPasswordTokenAsync(requestModel);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel requestModel)
    {
        var response = await _accountService.ResetPasswordAsync(requestModel);
        return Ok(response);
    }

    [HttpPost("ChangePhoneNumber")]
    public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumberRequestModel requestModel)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.ChangePhoneNumberAsync(requestModel, userId);
        return Ok(response);
    }

    [HttpPost("ChangePhoneNumberConfirmWithToken")]
    public async Task<IActionResult> ChangePhoneNumberConfirmWithToken([FromBody] ConfirmPhoneNumberRequestModel requestModel)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.ConfirmPhoneNumberAsync(requestModel, userId);
        return Ok(response);
    }

    [HttpPost("ConfirmPhoneNumberToken")]
    public async Task<IActionResult> ConfirmPhoneNumberToken([FromBody] ChangePhoneNumberRequestModel requestModel)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.GeneratePhoneNumberTokenAsync(requestModel, userId);
        return Ok(response);
    }

    [HttpPost("ChangeEmail")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequestModel requestModel)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.ChangeEmailAsync(requestModel, userId);
        return Ok(response);
    }

    [HttpPost("ChangeEmailConfirmWithToken")]
    public async Task<IActionResult> ChangeEmailConfirmWithToken([FromBody] ConfirmEmailRequestModel requestModel)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.ChangeEmailConfirmWithToken(requestModel, userId);
        return Ok(response);
    }

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser(UserRequestModel model)
    {
        var userId = User.GetClaimUserId();
        return Ok(await _accountService.AddUserInfo(model, userId));
    }

    [HttpPut("UpdateUser/{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserRequestModel model)
    {
        var userId = User.GetClaimUserId();
        return Ok(await _accountService.UpdateUserInfo(id, model, userId));
    }

    [HttpPost("UserDelete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        return Ok(await _accountService.DeleteUserInfo(id));
    }

    [HttpPost("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers([FromBody] GetUserWithFilter model)
    {
        var route = Request.Path.Value;
        return Ok(await _accountService.GetAllUsersAsync(model, route));
    }

    [HttpGet("GetProfileByUserId/{userId}")]
    public async Task<IActionResult> GetProfileByUserId(string userId)
    {
        //var userId = User.GetClaimUserId();
        var response = await _accountService.GetProfile(userId);
        return Ok(response);
    }

    [HttpGet("MyProfile")]
    public async Task<IActionResult> MyProfile()
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.GetProfile(userId);
        return Ok(response);
    }

    [HttpGet("GetUserRoleById/{userId}")]
    public async Task<IActionResult> getUserRoleById(string userId)
    {
        var response = await _accountService.GetRolesByUserIdAsync(userId);
        return Ok(response);
    }

    [HttpGet("AddUserToRole/{role}")]
    public async Task<IActionResult> AddUserToRole(string role)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.AddUserToRoleAsync(role, userId);
        return Ok(response);
    }

    [HttpGet("RemoveUserFromRole/{role}")]
    public async Task<IActionResult> RemoveUserFromRole(string role)
    {
        var userId = User.GetClaimUserId();
        var response = await _accountService.RemoveUserFromRoleAsync(role, userId);
        return Ok(response);
    }

    [HttpPost("AddUserToRoles")]
    public async Task<IActionResult> AddUserToRoles([FromBody] AddUserToRolesRequest request)
    {
        var response = await _accountService.AddUserToRolesAsync(request, request.UserName);
        return Ok(response);
    }

    [HttpPost("RemoveUserFromRoles")]
    public async Task<IActionResult> RemoveUserFromRoles([FromBody] AddUserToRolesRequest request)
    {
        var response = await _accountService.RemoveUserFromRolesAsync(request, request.UserName);
        return Ok(response);
    }

    [HttpPost("GetUserClaims")]
    public async Task<IActionResult> GetUserClaims(GetUserClaimsRequestModel request)
    {
        var response = await _accountService.GetUserClaimsAsync(request);
        return Ok(response);
    }

    [HttpPost("GetUserRoles")]
    public async Task<IActionResult> GetUserRoles(GetUserRolesRequestModel request)
    {
        var response = await _accountService.GetUserRolesAsync(request);
        return Ok(response);
    }

    [HttpGet("GetAllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        return Ok(await _accountService.GetAllRolesAsync());
    }

    [HttpGet("GetCurrentUserClaims")]
    public async Task<IActionResult> GetAllUserClaims()
    {
        List<ClaimListViewModel> newClaimList = new List<ClaimListViewModel>();
        var claimList = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
        foreach (var aClaim in claimList)
        {
            var aInfo = new ClaimListViewModel()
            {
                Type = aClaim.Type,
                Value = aClaim.Value
            };
            newClaimList.Add(aInfo);
        }
        return Ok(newClaimList); //await _accountService.GetAllUserClaimsAsync()
    }

    [HttpGet("GetAllRoleClaims")]
    public async Task<IActionResult> GetAllRoleClaims()
    {
        return Ok(await _accountService.GetAllRoleClaimsAsync());
    }

    [HttpPost("EditRole")]
    public async Task<IActionResult> AddNewRole(EditRoleNameRequestModel req)
    {
        return Ok(await _accountService.EditRole(req));
    }

    [HttpGet("DeleteRole/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        return Ok(await _accountService.DeleteRole(id));
    }

}