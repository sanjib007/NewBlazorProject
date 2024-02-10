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

namespace L3T.FieldForceApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class RsmChecklistController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RsmChecklistController(IMediator mediator)
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

        [HttpGet("AllDataForRSMCheckList/{clientId}")]
        public async Task<IActionResult> AllDataForRSMCheckList(string clientId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new AllDataForRSMCheckListQuery() { clientId = clientId, userId = getUserid, ip = ip });
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

        
        [HttpGet("GetRsmChecklist")]
        public async Task<IActionResult> GetRsmChecklist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmChecklistQuery() { ip = ip, userId = getUserid });
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


        
        [HttpGet("GetRsmRouterTypelist")]
        public async Task<IActionResult> GetRsmRouterTypelist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmRouterTypeQuery() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmControllerOwnerlist")]
        public async Task<IActionResult> GetRsmControllerOwnerlist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmControllerOwnerQuery() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmSingleAplist")]
        public async Task<IActionResult> GetRsmSingleAplist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmSingleApQuery() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmMultipleAplist")]
        public async Task<IActionResult> GetRsmMultipleAplist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmMultipleApQuery() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmChannelWidth20MHzlist")]
        public async Task<IActionResult> GetRsmChannelWidth20MHzlist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmChannelWidth20MHzQuery() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmGhzEnabledlist")]
        public async Task<IActionResult> GetRsmGhzEnabledlist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmGhzEnabledQuery() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmChannelWidthAutolist")]
        public async Task<IActionResult> GetRsmChannelWidthAutolist()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmChannelWidthAutoQuery() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmChannelbetween149_161list")]
        public async Task<IActionResult> GetRsmChannelbetween149_161list()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmChannelbetween149_161Query() { ip = ip, userId = getUserid });
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

        
        [HttpGet("GetRsmChecklistInfo/{clientId}")]
        public async Task<IActionResult> GetRsmChecklistInfo(string clientId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetRsmChecklistDetailsByClientIdQuery() { clientId = clientId, ip = ip, userId = getUserid });
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


        [HttpPost("SaveRsmChecklistDetails")]
        public async Task<IActionResult> SaveRsmChecklistDetails([FromForm] RsmCheckListRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new UpdateRsmChecklistDataCommand() { model = model, userId = getUserid, ip = ip });
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
