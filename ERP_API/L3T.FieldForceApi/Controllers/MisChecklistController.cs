using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
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
    public class MisChecklistController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MisChecklistController(IMediator mediator)
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


        [AllowAnonymous]
        [HttpGet("GetMisChecklistInfo/{ticketId}")]
        public async Task<IActionResult> GetMisChecklistInfoByTicketId(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetMisChecklistDetailsByTicketIdQuery() { ticketId = ticketId, ip = ip });
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


        [AllowAnonymous]
        [HttpGet("GetCustomerInfo/{ticketId}")]
        public async Task<IActionResult> GetCustomerInfoByTicketId(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetCustomerInfoByTicketIdQuery() { ticketId = ticketId, ip = ip });
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



        [AllowAnonymous]
        [HttpGet("GetCheckList")]
        public async Task<IActionResult> GetChecklist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetChecklistQuery() { ip = ip });
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


        [AllowAnonymous]
        [HttpGet("GetRouterTypelist")]
        public async Task<IActionResult> GetRouterTypelist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetRouterTypeQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetControllerOwnerlist")]
        public async Task<IActionResult> GetControllerOwnerlist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetControllerOwnerQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetSingleAplist")]
        public async Task<IActionResult> GetSingleAplist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetSingleApQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetMultipleAplist")]
        public async Task<IActionResult> GetMultipleAplist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetMultipleApQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetChannelWidth20MHzlist")]
        public async Task<IActionResult> GetChannelWidth20MHzlist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetChannelWidth20MHzQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetGhzEnabledlist")]
        public async Task<IActionResult> GetGhzEnabledlist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetGhzEnabledQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetChannelWidthAutolist")]
        public async Task<IActionResult> GetChannelWidthAutolist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetChannelWidthAutoQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetChannelbetween149_161list")]
        public async Task<IActionResult> GetChannelbetween149_161list()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetChannelbetween149_161Query() { ip = ip });
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


        [AllowAnonymous]
        [HttpPost("SaveChecklist")]
        public async Task<IActionResult> SaveChecklistDetails([FromForm] MisCheckListRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new UpdateChecklistDataCommand() { model = model, ip = ip });
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

            //return null;
        }

        [AllowAnonymous]
        [HttpPost("UploadSingleFile")]
        public async Task<IActionResult> UploadSingleFile([FromForm] FileUploadModel uploadModel)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var user = User;
                var meidatorObj = await _mediator.Send(new CheckListFileUploadCommand() { model = uploadModel, user = user, ip = ip });
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


        [AllowAnonymous]
        [HttpGet("GetCustomerInfoB2B/{ticketId}")]
        public async Task<IActionResult> GetCustomerInfoB2BByTicketId(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetCustomerInfoByTicketIdQuery() { ticketId = ticketId, ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetMisChecklistInfoB2B/{ticketId}")]
        public async Task<IActionResult> GetMisChecklistInfoB2BByTicketId(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetMisChecklistDetailsB2BByTicketIdQuery() { ticketId = ticketId, ip = ip });
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

        [AllowAnonymous]
        [HttpGet("GetCheckListB2B")]
        public async Task<IActionResult> GetChecklistB2B()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetChecklistB2BQuery() { ip = ip });
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

        [AllowAnonymous]
        [HttpPost("SaveChecklistB2B")]
        public async Task<IActionResult> SaveChecklistB2BDetails([FromForm] MisCheckListRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new UpdateChecklistB2BDataCommand() { model = model, ip = ip });
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

            //return null;
        }
    }
}
