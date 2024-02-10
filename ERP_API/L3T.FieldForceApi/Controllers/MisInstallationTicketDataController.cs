using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.FieldForceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class MisInstallationTicketDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MisInstallationTicketDataController(IMediator mediator)
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

        [HttpGet("GetAssignedMisInstallationTicketList")]
        public async Task<ActionResult> GetAssignedMisInstallationTicketList(string userId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var user = User;
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                //var getUserid = userId;
                var meidatorObj = await _mediator.Send(new MISInstallationTicketListQuery()
                {
                    user = user,
                    userId = getUserid,
                    ip = ip
                });
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

            //return Ok(null);
        }

        //[HttpGet("GetAssignedMisInstallationTicketInfo")]
        //public async Task<ActionResult> GetAssignedMisInstallationTicketInfo(string ticketId)
        //{
        //    try
        //    {
        //        var ip = await GetClientIPAddress();
        //        var user = User;
        //        var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
        //        var meidatorObj = await _mediator.Send(new MISInstallationTicketListQuery()
        //        {
        //            userId = getUserid,
        //            ip = ip
        //        });
        //        return Ok(meidatorObj);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse()
        //        {
        //            Status = "Error",
        //            StatusCode = 400,
        //            Message = ex.Message
        //        });
        //    }
        //}

        [HttpGet("GetNetworkTypeList")]
        public async Task<ActionResult> GetNetworkTypeList()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var user = User;
                var meidatorObj = await _mediator.Send(new NetworkTypeQuery()
                {
                    user = user,
                    ip = ip
                });
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

        [HttpGet("GetClientDatabaseMainInfoByAddressCode")]
        public async Task<IActionResult> GetClientDatabaseMainInfoByAddressCode(string clientAddressCode)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ClientDatabaseMainInfoByAddressCodeQuery()
                {
                    user = user,
                    ip = ip,
                    clientAddressCode = clientAddressCode
                });
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

        [HttpGet("GetClientBillingAddressInfo")]
        public async Task<IActionResult> GetClientBillingAddressInfo(string brClientCode, int brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ClientBillingAddressInfoQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = brSerialNumber
                });
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

        [HttpGet("GetClientTechnicalInfo")]
        public async Task<IActionResult> GetClientTechnicalInfo(string brClientCode, int brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ClientTechnicalInfoQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = brSerialNumber
                });
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

        [HttpGet("GetInstallationCompletionData")]
        public async Task<IActionResult> GetInstallationCompletionData(string brClientCode, int brSerialNumber, string clientName)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new InstallationCompletionDataQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = brSerialNumber,
                    clientName = clientName
                });
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

        [HttpGet("GetPendingTicketPriorityList")]
        public async Task<IActionResult> GetTicketPriorityList(string ticketId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new TicketPriorityListQuery()
                {
                    user = user,
                    ip = ip,
                    ticketId = ticketId
                });
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

        [HttpPost("UpdateTicketPriorityStstus")]
        public async Task<IActionResult> UpdateTicketPriorityStstus(string userId, string ticketId,
            int priorityStatus, int pendingListSlNo, string serviceType)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new UpdateTicketPriorityStatusCommand()
                {
                    userId = userId,
                    ip = ip,
                    ticketId = ticketId,
                    priorityStatus = priorityStatus,
                    pendingListSlNo = pendingListSlNo,
                    serviceType = serviceType
                });
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


        [HttpGet("GetMailLogByTicketId")]
        public async Task<IActionResult> GetMailLogByTicketId(string ticketId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new MailLogQuery()
                {
                    user = user,
                    ip = ip,
                    ticketId = ticketId
                });
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

        [HttpGet("GetParentData")]
        public async Task<IActionResult> GetParentData()
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new MailLogQuery()
                {
                    user = user,
                    ip = ip

                });
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
