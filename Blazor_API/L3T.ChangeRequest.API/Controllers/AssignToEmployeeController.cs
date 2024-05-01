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
    public class AssignToEmployeeController : CustomsBaseController
    {
        private readonly IAssignEmployeeService _assignEmployeeService;
        public AssignToEmployeeController(
            IBaseControllerCommonService cRMenuAndPermissionSetupService,
            IAssignEmployeeService assignEmployeeService) : base(cRMenuAndPermissionSetupService)
        {
            _assignEmployeeService = assignEmployeeService;
        }
        

        [HttpPost("GetAssignEmployeeAll")]
        public async Task<IActionResult> GetAssignEmployeeAll(AssignEmployeeListReqModel ReqModel)
        {
            var route = Request.Path.Value;
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.GetAllAssignEmployeeQuery(ReqModel, route, getUserid, ip);
            return await responseCheck(result);
        }


        [HttpGet("AllAssignDeveloper")]
        public async Task<IActionResult> GetAllAssignDeveloper()
        {
            var route = Request.Path.Value;
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.GetAllAssignDeveloper(getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("ReloadAllAssignDeveloper")]
        public async Task<IActionResult> UpdateAllAssignDeveloper()
        {
            var route = Request.Path.Value;
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.UpdateAllAssignDeveloper(getUserid, ip);
            return await responseCheck(result);
        }


        [HttpGet("AssignEmployeeByCrId/{crId}")]
        public async Task<IActionResult> GetAssignEmployeeById(long crId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.GetAssignEmployeeByCrId(crId, getUserid, ip);
            return await responseCheck(result);

        }


        [HttpPost("AddAssignEmployee")]
        public async Task<IActionResult> AddAssignEmployee(AddAssignEmployeeReq ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.AddAssignEmployee(ReqModel, getUserid, ip);
            return await responseCheck(result);

        }



        [HttpPut("UpdateAssignEmployee")]
        public async Task<IActionResult> UpdateAssignEmployee(UpdateAssignEmployeeReq ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.UpdateAssignEmployee(ReqModel, getUserid, ip);
            return await responseCheck(result);

        }


        [HttpDelete("AssignEmployeeDelete/{id}")]
        public async Task<IActionResult> AssignEmployeeDelete(long id)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.AssignEmployeeDelete(id, getUserid, ip);
            return await responseCheck(result);

        }

        [HttpGet("UpdateAssignEmployeeTaskStatusIsCompleteOrIncomplete/{crId}")]
        public async Task<IActionResult> UpdateAssignEmployeeTaskStatusIsCompleteOrIncomplete(long crId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _assignEmployeeService.UpdateAssignEmployeeTaskStatusIsCompleteOrIncomplete(crId, getUserid, ip);
            return await responseCheck(result);

        }


    }
}
