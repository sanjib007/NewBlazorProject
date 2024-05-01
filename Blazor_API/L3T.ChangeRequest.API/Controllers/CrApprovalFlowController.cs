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
    public class CrApprovalFlowController : CustomsBaseController
    {
        private readonly ICrApprovalFlowService _crApprovalFlowService;
        public CrApprovalFlowController(
            IBaseControllerCommonService cRMenuAndPermissionSetupService,
            ICrApprovalFlowService crApprovalFlowService) : base(cRMenuAndPermissionSetupService)
        {
            _crApprovalFlowService = crApprovalFlowService;
        }


        [HttpGet("CrApprovalFlowByCrId/{CrId}")]
        public async Task<IActionResult> CrApprovalFlowByCrId(long CrId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crApprovalFlowService.GetCrApprovalFlow(CrId, getUserid, ip);
            return await responseCheck(result);
        }


        [HttpGet("CrApprovedApproverList")]
        public async Task<IActionResult> CrApprovedApproverList(long CrId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crApprovalFlowService.CrApprovedApproverList(CrId, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("OnlyApprovedDataByCrId/{CrId}")]
        public async Task<IActionResult> OnlyApprovedDataByCrId(long CrId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crApprovalFlowService.GetCrApprovalFlow(CrId, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("AddCrApprovalFlow")]
        public async Task<IActionResult> AddCrApprovalFlow(AddCrApprovalFlowReq ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            ReqModel.ApproverName = User.GetClaimUserFullName();
            ReqModel.ApproverDesignation = User.GetClaimUserDesignation();
            ReqModel.ApproverDepartment = User.GetClaimUserDepartment();
            ReqModel.ApproverEmpId = User.GetClaimUserL3Id();
            var result = await _crApprovalFlowService.AddCrApprovalFlow(ReqModel, getUserid, ip);
            return await responseCheck(result);
        }

       
        [HttpPut("UpdateCrApprovalFlow")]
        public async Task<IActionResult> UpdateCrApprovalFlow( UpdateCrApprovalFlowReq updateReq)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crApprovalFlowService.UpdateCrApprovalFlow(updateReq, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpDelete("DeleteCrApprovalFlow/{id}")]
        public async Task<IActionResult> DeleteCrApprovalFlow(long id)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crApprovalFlowService.CrApprovalFlowDelete(id, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("AddRemark")]
        public async Task<IActionResult> AddRemark(AddRemarkRequestModel reqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _crApprovalFlowService.AddRemark(reqModel, getUserid, ip);
            return await responseCheck(result);
        }
    }
}
