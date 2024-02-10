using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Data;

namespace L3T.FieldForceApi.Controllers.RSM
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class RSMComplainTicketController : Controller
    {
        private readonly IMediator _mediator;
        public RSMComplainTicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<string> GetClientIPAddress()
        {
            var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var localIpAddress = httpConnectionFeature?.LocalIpAddress;
            var remoteIpAddress = httpConnectionFeature?.RemoteIpAddress;
            var ip = localIpAddress.ToString() + "/" + remoteIpAddress.ToString();
            return ip;
        }

        [HttpGet("MyTask")]
        public async Task<IActionResult> MyTask()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new RSMComplainTicketMyTaskQuery() { userId = getUserid, ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("SubscriberInformation/{customerId}")]
        public async Task<IActionResult> SubscriberInformation(string customerId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new RSMSubcriberInformationQuery() { customerId= customerId, userId = getUserid, ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("ComplainLogs/{ticketId}")]
        public async Task<IActionResult> ComplainLogs(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new RSMComplainLogsQuery() { TicketId = ticketId, userId = getUserid, ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("balanceAndPackageInfo/{customerId}")]
        public async Task<IActionResult> BalanceAndPackageInfo(string customerId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new HydraBalanceAndPackageInfoQuery() { CustomerId = customerId, UserId = getUserid, Ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("TechnicalInfoInfo/{customerId}")]
        public async Task<IActionResult> TechnicalInfoInfo(string customerId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new TechnicalInfoFromHydraQuery() { CustomerId = customerId, UserId = getUserid, Ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("AllImportentData/{ticketId}")]
        public async Task<IActionResult> AllImportentData(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new ComplainTicektImportentDataQuery() { TicketId = ticketId, userId = getUserid, ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("closeNature/{categoryId}")]
        public async Task<IActionResult> AllImportentData(int categoryId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new CloseNatureListQuery() { categoryId = categoryId, userId = getUserid, ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("closeTicket")]
        public async Task<IActionResult> closeTicket(RSMCloseTicketRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new CloseTicketCommand() { model = model, userId = getUserid, ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("ComplainPostReplay")]
        public async Task<IActionResult> ComplainPostReplay(PostReplayRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new ComplainPostReplayCommand() { model = model, userId = getUserid, ip = ip });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }
    }
}
