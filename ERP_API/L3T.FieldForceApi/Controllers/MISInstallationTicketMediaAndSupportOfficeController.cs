using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
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
    public class MISInstallationTicketMediaAndSupportOfficeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MISInstallationTicketMediaAndSupportOfficeController(IMediator mediator)
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

        [HttpGet("GetInternetTechnologyDatainMedia")]
        public async Task<IActionResult> GetInternetTechnologyDatainMedia()
        {
            var user = User;
            var ip = await GetClientIPAddress();

            try
            {
                var meidatorObj = await _mediator.Send(new MediaAndsupportDataQuery()
                {
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetDatabseTechnologySetupData")]
        public async Task<IActionResult> GetDatabseTechnologySetupData()
        {
            var user = User;
            var ip = await GetClientIPAddress();

            try
            {
                var meidatorObj = await _mediator.Send(new ClientDatabseTechnologySetupQuery()
                {
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost("GetClientDatabaseMediaSetupData")]
        public async Task<IActionResult> GetClientDatabaseMediaSetupData()
        {
            var user = User;
            var ip = await GetClientIPAddress();

            try
            {
                var meidatorObj = await _mediator.Send(new ClientDatabaseMediaSetupQuery()
                {
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("GetSupportOfficeData")]
        public async Task<IActionResult> GetSupportOfficeData()
        {
            var user = User;
            var ip = await GetClientIPAddress();

            try
            {
                var meidatorObj = await _mediator.Send(new GetSupportOfficeDataQuery()
                {
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("UpdateMediaInformation")]
        public async Task<IActionResult> UpdateMediaInformation(MediaInformationRequestModel requestModel)
        {
            var user = User;
            var ip = await GetClientIPAddress();

            try
            {
                var meidatorObj = await _mediator.Send(new InstallationMediaInfoUpdateCommand()
                {
                    model = requestModel,
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("SupportofficeDataUpdateInClientDatabase")]
        public async Task<IActionResult> SupportofficeDataUpdateInClientDatabase(
            SupportOfficeInfoRequestModel requestModel)
        {
            var user = User;
            var ip = await GetClientIPAddress();
            try
            {
                var meidatorObj = await _mediator.Send(new SupportofficeDataUpdateCommand()
                {
                    model = requestModel,
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
