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
    public class CrStatusController : CustomsBaseController
    {
        private readonly ICrStatusService _crStatusService;
        public CrStatusController(
            IBaseControllerCommonService cRMenuAndPermissionSetupService, 
            ICrStatusService crStatusService
            ) :base(cRMenuAndPermissionSetupService)
        {
            _crStatusService = crStatusService;
        }


        [HttpGet("AllStatus")]
        public async Task<IActionResult> GetAllStatusd()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crStatusService.GetAllStatus(getUserid, ip);
            return await responseCheck(result);

        }

        [HttpPost("AddStatus")]
        public async Task<IActionResult> AddStatus([FromForm] AddStatusReq ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crStatusService.AddStatus(ReqModel, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromForm] UpdateStatusReq updateChangeReq)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var getUserRole = User.GetClaimUserRoles();
            var result = await _crStatusService.UpdateStatus(updateChangeReq, getUserid, ip);
            return await responseCheck(result);

        }

        [HttpDelete("DeleteStatus/{id}")]
        public async Task<IActionResult> DeleteStatus(long id)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crStatusService.DeleteStatus(id, getUserid, ip);
            return await responseCheck(result);

        }

        [HttpGet("UpdateCRApprovedStatus/{crId}")]
        public async Task<IActionResult> UpdateCRApprovedStatus(long crId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var email = User.GetClaimUserEmail();
            var newRequest = new UpdateCrStatusRequestModel()
            {
                CrId = crId,
                CrStatus = AllStatus.Approved.ToString(),
                Email = email,
            };
            var result = await _crStatusService.UpdateCRStatus(newRequest, getUserid, ip);
            
            return await responseCheck(result);

        }


        [HttpGet("UpdateCRRejectedStatus/{crId}")]
        public async Task<IActionResult> UpdateCRRejectedStatus(long crId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var email = User.GetClaimUserEmail();
            var newRequest = new UpdateCrStatusRequestModel()
            {
                CrId = crId,
                CrStatus = AllStatus.Rejected.ToString(),
                Email = email,
            };
            var result = await _crStatusService.UpdateCRStatus(newRequest, getUserid, ip);
            return await responseCheck(result);

        }

        [HttpGet("UpdateCRCompletedStatus/{crId}")]
        public async Task<IActionResult> UpdateCRCompletedStatus(long crId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var email = User.GetClaimUserEmail();
            var newRequest = new UpdateCrStatusRequestModel()
            {
                CrId = crId,
                CrStatus = AllStatus.Completed.ToString(),
                Email = email,
            };
            var result = await _crStatusService.UpdateCRProcessStatus(newRequest, getUserid, ip);
            return await responseCheck(result);

        }

        [HttpGet("UpdateCROnHoldStatus/{crId}")]
        public async Task<IActionResult> UpdateCROnHoldStatus(long crId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var email = User.GetClaimUserEmail();
            var newRequest = new UpdateCrStatusRequestModel()
            {
                CrId = crId,
                CrStatus = AllStatus.OnHold.ToString(),
                Email = email,
            };
            var result = await _crStatusService.UpdateCRProcessStatus(newRequest, getUserid, ip);
            return await responseCheck(result);

        }

        [HttpGet("UpdateCROnProgressStatus/{crId}")]
        public async Task<IActionResult> UpdateCROnProgressStatus(long crId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var email = User.GetClaimUserEmail();
            var newRequest = new UpdateCrStatusRequestModel()
            {
                CrId = crId,
                CrStatus = AllStatus.InProgress.ToString(),
                Email = email,
            };
            var result = await _crStatusService.UpdateCRProcessStatus(newRequest, getUserid, ip);
            return await responseCheck(result);

        }

    }
}
