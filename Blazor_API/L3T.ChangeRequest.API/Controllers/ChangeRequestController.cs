using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;

namespace L3T.ChangeRequest.API.Controllers
{

    [ApiController]
    [Route(CommonHelper.CrApiControllerRoute)]
    [Authorize]
    public class ChangeRequestController : CustomsBaseController
    {
        private readonly IChangeRequestedInfoService _changeRequestedInfoService;
        public ChangeRequestController(
            IBaseControllerCommonService cRMenuAndPermissionSetupService, 
            IChangeRequestedInfoService changeRequestedInfoService
        ) : base(cRMenuAndPermissionSetupService)
        {
            _changeRequestedInfoService = changeRequestedInfoService;
        }

        [AllowAnonymous]
        [HttpGet("testMethod")]
        public async Task<IActionResult> testMethod()
        {
            return Ok(await _changeRequestedInfoService.testMethod("test data"));
        }
        
        [HttpGet("AddChangeRequest")]
        public async Task<IActionResult> AddChangeRequest()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _changeRequestedInfoService.AddChangeRequest(getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("ChangeRequestList")]
        public async Task<IActionResult> ChangeRequestList(ChangeRequestListReqModel ReqModel)
        {
            var test = ReqModel.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => prop.GetValue(ReqModel, null));


            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var route = Request.Path.Value;
            var role = User.GetClaimUserRoles();
            if(role != "Admin" && role != "SuperAdmin")
            {
                ReqModel.Department = User.GetClaimUserDepartment();
            }
            var result = await _changeRequestedInfoService.GetAllChangeRequest(ReqModel, route, getUserid, ip);
            return await responseCheck(result);

        }
        
        [HttpPost("PendingChangeRequestList")]
        public async Task<IActionResult> PendingChangeRequestList(ChangeRequestListReqModel ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var route = Request.Path.Value;
            var role = User.GetClaimUserRoles();
            ReqModel.ApproverEmpId = getUserid;
            ReqModel.Status = AllStatus.Submitted.ToString();
            var result = await _changeRequestedInfoService.GetAllChangeRequest(ReqModel, route, getUserid, ip);
            return await responseCheck(result);

        }

        [HttpGet("StatusWiseTotalCR")]
        public async Task<IActionResult> StatusWiseTotalCR()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var role = User.GetClaimUserRoles();
            var department = string.Empty;
            if (role != "Admin" && role != "SuperAdmin")
            {
                department = User.GetClaimUserDepartment();
            }
            var result = await _changeRequestedInfoService.GetStatusWiseTotalCR("StatusWiseTotalCR", department, getUserid, ip);
            return await responseCheck(result);
        }


        [HttpPost("MySubmitedCrList")]
        public async Task<IActionResult> MySubmitedCrList(ChangeRequestListReqModel ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var route = Request.Path.Value;
            ReqModel.UserId = getUserid;

            var result = await _changeRequestedInfoService.GetAllChangeRequest(ReqModel, route, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("MyCrTotalStatus")]
        public async Task<IActionResult> MyCrTotalStatus()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var role = User.GetClaimUserRoles();
            var result = await _changeRequestedInfoService.GetStatusWiseTotalCR("MyCrTotalStatus", "", getUserid, ip);
            return await responseCheck(result);

        }

        [HttpPost("MyApprovedCrList")]
        public async Task<IActionResult> MyApprovedCrList(ChangeRequestListReqModel ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var route = Request.Path.Value;
            ReqModel.UserId = getUserid;
            ReqModel.ApproverStatus = AllStatus.Approved.ToString();
            var result = await _changeRequestedInfoService.CrUserPersonalStatus(ReqModel, route, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("MyApprovedCrTotalStatus")]
        public async Task<IActionResult> MyApprovedCrTotalStatus()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var role = User.GetClaimUserRoles();
            var result = await _changeRequestedInfoService.GetStatusWiseTotalCR("MyApprovedCrTotalStatus", "", getUserid, ip);
            return await responseCheck(result);

        }


        [HttpPost("DeveloperAssignedCrList")]
        public async Task<IActionResult> DeveloperAssignedCrList(ChangeRequestListReqModel ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var route = Request.Path.Value;
            ReqModel.UserId = getUserid;
            var result = await _changeRequestedInfoService.CrForDeveloperWise(ReqModel, route, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("DeveloperCrTotalStatus")]
        public async Task<IActionResult> DeveloperCrTotalStatus()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var role = User.GetClaimUserRoles();
            var result = await _changeRequestedInfoService.GetStatusWiseTotalCR("DeveloperCrTotalStatus", "", getUserid, ip);
            return await responseCheck(result);

        }

        [HttpGet("StatusList")]
        public IActionResult GetStatusList()
        {
            var ip = GetClientIPAddress();
            return Ok(Enum.GetNames(typeof(AllStatus)).ToList());
        }

        [HttpGet("GetCrInfoFroDashboard")]
        public async Task<IActionResult> GetCrInfoFroDashboard()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var role = User.GetClaimUserRoles();
            var result = await _changeRequestedInfoService.GetAllCrTotalByCategory(getUserid, ip);
            var last5Cr = await _changeRequestedInfoService.GetLastFiveCr(getUserid, ip);
            var response = new ApiResponse()
            {
                Status = "Success",
                Message = "",
                StatusCode = 200,
                Data = new CrDashboardResponseModel()
                {
                    getAllTotalCrByCatagoryWises = result,
                    LastFiveCrInfo = last5Cr
                }
            };
            return await responseCheck(response);
        }

        [HttpGet("GetAllFiles/{crid}")]
        public async Task<IActionResult> GetAllFiles(long crid)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var role = User.GetClaimUserRoles();
            var result = await _changeRequestedInfoService.GetAllFiles(crid, getUserid, ip);
            return await responseCheck(result);
        }



    }
}
