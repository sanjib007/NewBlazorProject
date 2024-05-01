using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.RequestModel.Permission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.ChangeRequest.API.Controllers
{
    [ApiController]
    [Route(CommonHelper.CrApiControllerRoute)]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = CommonHelper.AllRole)]
    public class MenuSetupAndPermissionController : CustomsBaseController
    {
        private readonly IMenuAndPermissionSetupService _cRMenuAndPermissionService;
        private readonly IConfiguration _configuration;
        public MenuSetupAndPermissionController(IBaseControllerCommonService cRMenuAndPermissionSetupServiceAdd,
             IMenuAndPermissionSetupService cRMenuAndPermissionService, 
            IConfiguration configuration) 
            : base(cRMenuAndPermissionSetupServiceAdd)
        {
            _cRMenuAndPermissionService = cRMenuAndPermissionService;
            _configuration = configuration;
        }

        [HttpGet("GetAllMenuAndPermission")]
        public async Task<IActionResult> GetAllMenuAndPermission()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var getRole = User.GetClaimUserRoles();
            string projectName = _configuration.GetValue<string>("DefaultApproverDepartment:ProjectName");

            var request = new GetAllMenuSetupAndPermissionRequestModel()
            {
                projectName = projectName,
                roleName = getRole,
            };

            return Ok(await _cRMenuAndPermissionService.SpecificUserAndRoleWiseGetAllMenuAndPermission(request, ip, getUserid));
        }

        [HttpPost("getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise")]
        public async Task<IActionResult> getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise(GetAllMenuSetupAndPermissionRequestModel model)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            model.projectName = _configuration.GetValue<string>("DefaultApproverDepartment:ProjectName");

            return Ok(await _cRMenuAndPermissionService.getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise(model, ip, getUserid));
        }

        [HttpGet("getAllMenuAndPermissionSetup")]
        public async Task<IActionResult> getAllMenuAndPermissionSetup()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            string projectName = _configuration.GetValue<string>("DefaultApproverDepartment:ProjectName");

            return Ok(await _cRMenuAndPermissionService.getMenuAndPermissionSetup(projectName, ip, getUserid));
        }

        [HttpPost("SingleMenuUpdate")]
        public async Task<IActionResult> SingleMenuUpdate(MenuSetupRequestModel model)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            return Ok(await _cRMenuAndPermissionService.SingleMenuUpdate(model, ip, getUserid));
        }

        [HttpPost("PermissionSetupRoleAndUserWise")]
        public async Task<IActionResult> PermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            return Ok(await _cRMenuAndPermissionService.PermissionSetupRoleAndUserWise(model, ip, getUserid));
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roleStr = CommonHelper.AllRole.ToString();
            var strList = roleStr.Split(',').Select(p=> p.Trim()).ToList();
            var response = new ApiResponse()
            {
                Status = "Success",
                StatusCode = 200,
                Message= "OK",
                Data = strList
            };
            return Ok(response);
        }

		[HttpPost("DeletePermissionSetupRoleAndUserWise")]
		public async Task<IActionResult> DeletePermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model)
		{
			var ip = await GetClientIPAddress();
			var getUserid = User.GetClaimUserId();
            model.MenuId = null;
			return Ok(await _cRMenuAndPermissionService.DeletePermissionSetupRoleAndUserWise(model, ip, getUserid));
		}

		



	}
}
