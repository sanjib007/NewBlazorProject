using L3T.Utility.Helper;
using L3TIdentityTokenServer.CommonModel;
using L3TIdentityTokenServer.DataAccess.IdentityModels;
using L3TIdentityTokenServer.DataAccess.RequestModel;
using L3TIdentityTokenServer.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3TIdentityTokenServer.Controllers;
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
    
    [HttpGet("RoleInsert/{name}")]
    public async Task<IActionResult> RoleInsert(string name)
    {
        var isHave = await _roleManager.RoleExistsAsync(name);
        if (isHave)
        {
            throw new ApplicationException("This role is already exist.");
        }

        var newRole = new AppRoles()
        {
            Name = name
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
    
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _accountService.GetAllUsersAsync());
    }
    
    [HttpGet("GetAllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        return Ok(await _accountService.GetAllRolesAsync());
    }
    
    [HttpGet("GetAllUserClaims")]
    public async Task<IActionResult> GetAllUserClaims()
    {
        var allClaim = User.Claims.ToList();
        return Ok(allClaim); //await _accountService.GetAllUserClaimsAsync()
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