using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.SelfCare.RequestModel;
using L3T.SelfcareAPI.CommandQuery.Command;
using L3T.SelfcareAPI.CommandQuery.Query;
using L3T.Utility.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Data;

namespace L3T.SelfcareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SelfCareController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SelfCareController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<string> GetClientIPAddress()
        {
            var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
            //var localIpAddress = httpConnectionFeature?.LocalIpAddress;
            var localIpAddress = httpConnectionFeature?.RemoteIpAddress;
            var ip = localIpAddress.ToString();
            return ip;
        }


        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("test")]
        public async Task<IActionResult> IndexTest()
        {
            try
            {
                var meidatorObj = await _mediator.Send(new testQuery() { Id = 5 });
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

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
		[HttpPost("create-service")]
        public async Task<IActionResult> CreateService([FromBody] ServiceCreateRequestModel requestModel)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var userId = User.GetClaimUserId();
                var meidatorObj = await _mediator.Send(new CreateServiceCommand() { model = requestModel, UserIP = ip, UserId = userId });
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