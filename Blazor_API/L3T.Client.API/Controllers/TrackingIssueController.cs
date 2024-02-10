using L3T.ChangeRequest.API.Controllers;
using L3T.Infrastructure.Helpers.Models.RequestModel.Client;
using L3T.Infrastructure.Helpers.Services.ServiceImplementation.Client;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace L3T.Client.API.Controllers
{

    [ApiController]
    [Route(CommonHelper.ControllerRoute)]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class TrackingIssueController : CustomsBaseController
    {
        private readonly ITrackingIssueService _trackingIssueService;
        public TrackingIssueController(ITrackingIssueService trackingIssueService)
        {
            _trackingIssueService = trackingIssueService;
        }

        [HttpGet("TrackingIssueList/{day}")]
        public async Task<IActionResult> GetTrackingIssue(int day)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _trackingIssueService.AllTicketInfo(day, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("TrackingIssueListByCustomerId/{customerId}")]
        public async Task<IActionResult> GetTrackingIssueByCustomer(string customerId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _trackingIssueService.AllTicketInfoByCustomer(customerId, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("GetTicketNatureList/{systemType}")]
        public async Task<IActionResult> GetTicketNature(string systemType)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _trackingIssueService.GetAllTicketsNature(systemType, getUserid, ip);
            return await responseCheck(result);
        }


        [HttpPost("CreateTicket")]
        public async Task<IActionResult> TicketCreate(TicketCreateReqModel ReqModel)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            
            var result = await _trackingIssueService.AddTicketRsmOrMis(ReqModel, getUserid, ip);
            return await responseCheck(result);
        }


        [HttpGet("HistoricalSteps/{tickeRrefNo}")]
        public async Task<IActionResult> GetHistoricalSteps(string tickeRrefNo)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _trackingIssueService.RSMComplainTicketLogs(tickeRrefNo, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("AssignedPackage/{customerId}")]
        public async Task<IActionResult> GetAssignedPackage(string customerId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _trackingIssueService.GetAssignedPackageByCustomer(customerId, getUserid, ip);
            return await responseCheck(result);
        }

    }

}
