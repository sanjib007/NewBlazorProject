using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.ChangeRequest.API.Controllers
{
    [ApiController]
    [Route(CommonHelper.CrApiControllerRoute)]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = CommonHelper.AllRole)]
    public class TempChangeRequestController : CustomsBaseController
    {
        private readonly ITempChangeRequestedInfoService _tempChangeRequestedInfoService;

        public TempChangeRequestController(
            IBaseControllerCommonService cRMenuAndPermissionSetupService, 
            ITempChangeRequestedInfoService tempChangeRequestedInfoService
            ) :base(cRMenuAndPermissionSetupService)
        {
            _tempChangeRequestedInfoService = tempChangeRequestedInfoService;
        }

        [HttpGet("TempChangeRequestById")]
        public async Task<IActionResult> GetTempChangeRequestById(long id)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _tempChangeRequestedInfoService.GetChangeRequirementById(id, getUserid, ip);
            return await responseCheck(result);
        }
        

        [HttpGet("UncompleteChangeRequest")]
        public async Task<IActionResult> UncompleteChangeRequest()
        {
            var userId = User.GetClaimUserId();
            var ip = await GetClientIPAddress();
            var result = await _tempChangeRequestedInfoService.UncompleteChangeRequest(userId, ip);
            return await responseCheck(result);
        }



        [HttpPost("AddTempChangeRequest")]
        public async Task<IActionResult> AddTempChangeRequest([FromForm] AddTempChangeRequestModel ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            ReqModel.RequestorName = User.GetClaimUserFullName();
            ReqModel.DepartName = User.GetClaimUserDepartment();
            ReqModel.RequestorDesignation = User.GetClaimUserDesignation();
            ReqModel.ContactNumber = User.GetClaimUserPhoneNo();
            ReqModel.Email = User.GetClaimUserEmail();
            ReqModel.EmployeeId = getUserid;
            var result = await _tempChangeRequestedInfoService.AddChangeRequirement(ReqModel, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("RemovedFile")]
        public async Task<IActionResult> RemovedFile(RemovedFileRequestModel model)
        {
            var userId = User.GetClaimUserId();
            var ip = await GetClientIPAddress();
            var result = await _tempChangeRequestedInfoService.RemovedFile(model, userId, ip);
            return await responseCheck(result);
        }

        [HttpPut("UpdateTempChangeRequest")]
        public async Task<IActionResult> UpdateChangeRequest([FromForm] AddTempChangeRequestModel updateChangeReq)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _tempChangeRequestedInfoService.UpdateChangeRequirement(updateChangeReq, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpDelete("DeleteTempChangeRequest/{id}")]
        public async Task<IActionResult> ChangeRequest(long id)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _tempChangeRequestedInfoService.ChangeRequirementDelete(id, getUserid, ip);
            return await responseCheck(result);

        }
    }
}
