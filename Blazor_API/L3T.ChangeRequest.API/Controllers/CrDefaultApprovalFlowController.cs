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
    [Route(CommonHelper.ControllerRoute)]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = CommonHelper.AllRole)]
    public class CrDefaultApprovalFlowController : CustomsBaseController
    {
        private readonly ICrDefaultApprovalFlowService _crDefaultApprovalFlowService;
        public CrDefaultApprovalFlowController(
            IBaseControllerCommonService cRMenuAndPermissionSetupService, 
            ICrDefaultApprovalFlowService crDefaultApprovalFlowService
            ) :base(cRMenuAndPermissionSetupService)
        {
            _crDefaultApprovalFlowService = crDefaultApprovalFlowService;
        }

        [HttpGet("GetAllDefaultApprovalFlow")]
        public async Task<IActionResult> GetAllDefaultApprovalFlow()
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crDefaultApprovalFlowService.GetAllDefaultApprovalFlow(getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("AddPrincipleDefaultApprovalFlowFor")]
        public async Task<IActionResult> AddPrincipleDefaultApprovalFlowFor(AddCrDefaultApprovalFlowReq ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            ReqModel.IsPrincipleApprover = true;
            var result = await _crDefaultApprovalFlowService.AddCrDefaultApprovalFlow(ReqModel, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("AddCrDefaultApprovalFlow")]
        public async Task<IActionResult> AddCrDefaultApprovalFlow(AddCrDefaultApprovalFlowReq ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            ReqModel.IsPrincipleApprover = false;
            var result = await _crDefaultApprovalFlowService.AddCrDefaultApprovalFlow(ReqModel, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("DefaultApproverActiveInactive")]
        public async Task<IActionResult> DefaultApproverActiveInactive(UpdateCrDefaultApprovalFlowReq updateReq)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            UpdateCrDefaultApprovalFlowReq updateData = new UpdateCrDefaultApprovalFlowReq()
            {
                Id = updateReq.Id,
                IsActive = updateReq.IsActive
            };
            var result = await _crDefaultApprovalFlowService.ActiveInactiveApprover(updateData, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("UpdateCrDefaultApprovalFlow")]
        public async Task<IActionResult> UpdateCrDefaultApprovalFlow(UpdateCrDefaultApprovalFlowReq updateReq)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crDefaultApprovalFlowService.UpdateCrDefaultApprovalFlow(updateReq, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpDelete("DeleteDefaultCrApprovalFlow/{id}")]
        public async Task<IActionResult> DeleteCrDefaultApprovalFlow(long id)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crDefaultApprovalFlowService.CrDefaultApprovalFlowDelete(id, getUserid, ip);
            return await responseCheck(result);
        }

    }
}
