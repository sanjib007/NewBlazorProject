using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission.RequestModel;
using L3T.OAuth2DotNet7.DataAccess.RequestModel;
using L3T.OAuth2DotNet7.DataAccess.ViewModel;
using L3T.OAuth2DotNet7.Services.Interface;
using L3T.OAuth2DotNet7.Services.Interface.MenuAndPermission;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.OAuth2DotNet7.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = CommonHelper.AllRole)]
public class AccountController : Controller
{
    private readonly RoleManager<AppRoles> _roleManager;
    private readonly IAccountService _accountService;
    private readonly IMenuAndPermissionSetupService _cRMenuAndPermissionService;
    private readonly IConfiguration _configuration;
    public AccountController(
        RoleManager<AppRoles> roleManager,
        IAccountService accountService,
        IMenuAndPermissionSetupService cRMenuAndPermissionService,
        IConfiguration configuration)
    {
        _roleManager = roleManager;
        _accountService = accountService;
        _cRMenuAndPermissionService = cRMenuAndPermissionService;
        _configuration = configuration;
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

	[HttpGet("GetAllUsersForSearch")]
	public async Task<IActionResult> GetAllUsersForSearch(string searchText)
	{
		return Ok(await _accountService.GetAllUsersForSearchStrListAsync(searchText));
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

    [HttpGet("AllDepartment")]
    public async Task<IActionResult> AllDepartment()
    {
        return Ok(await _accountService.GetAllDepartment());
    }

    [HttpGet("AllDepartmentWiseEmployee/{deptName}")]
    public async Task<IActionResult> AllDepartment(string deptName)
    {
        return Ok(await _accountService.GetAllDepartmentWiseEmployee(deptName));
    }

    [HttpPost("PreAssignForCR")]
    public async Task<IActionResult> PreAssignForCR(PreAssignForCRRequestModel model)
    {
        var l3UserId = User.GetClaimUserL3Id();
        return Ok(await _accountService.PreAssignForCR(model, l3UserId));
    }

    [HttpGet("SearchEmployee")]
    public async Task<IActionResult> SearchEmployee(string text)
    {
        return Ok(await _accountService.SearchEmployee(text));
    }
    [HttpGet("AllEmployee")]
    public async Task<IActionResult> GetAllEmployee()
    {
        return Ok(await _accountService.GetAllEmployee());
    }

    [HttpGet("GetAllMenuAndPermission")]
    public async Task<IActionResult> GetAllMenuAndPermission()
    {
        var getUserid = User.GetClaimUserId();
        var getRole = User.GetClaimUserRoles();
        string projectName = _configuration.GetValue<string>("DefaultApproverDepartment:ProjectName");

        var request = new GetAllMenuSetupAndPermissionRequestModel()
        {
            projectName = projectName,
            roleName = getRole,
        };
        var allMenuAndPermission = await _cRMenuAndPermissionService.getMenuAndPermissionSetup(projectName, null, getUserid);
        var setPermission = await _cRMenuAndPermissionService.SpecificUserAndRoleWiseGetAllMenuAndPermission(request, null, getUserid);

        var apiResponse = new ApiResponse()
        {
            Status = "Success",
            StatusCode = 200,
            Message = "Ok",
            Data = new PermissionAndSetupModel()
            {
                AllMenuAndPermissiona = (List<MenuSetupModel>)allMenuAndPermission.Data,
                SetupedPermission = (List<MenuSetupAndPermissionViewModel>)setPermission.Data
            }
        };

        return Ok(apiResponse);
    }
}

