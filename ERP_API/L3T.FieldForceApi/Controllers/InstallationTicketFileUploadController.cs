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
    public class InstallationTicketFileUploadController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InstallationTicketFileUploadController(IMediator mediator)
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

        [HttpPost("UploadMisInstallationTicketFile")]
        public async Task<ActionResult> UploadMisInstallationTicketFile(string file_header,
            [FromForm] FileUploadModel fileDetails, string cli_code, Int32 code_sl,
            string file_categry_tki_number, string upload_by, string ServiceID
             )
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new InstallationFileUploadCommand()
                {
                    fileDetails = fileDetails,
                    user = user,
                    ip = ip,
                    file_header = file_header,
                    cli_code = cli_code,
                    code_sl = code_sl,
                    file_categry_tki_number = file_categry_tki_number,
                    upload_by = upload_by,
                    ServiceID = ServiceID
                });
                return Ok(meidatorObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("UploadedFileList")]
        public async Task<ActionResult> UploadedFileList(string tki_number)
        {
            if (string.IsNullOrWhiteSpace(tki_number))
            {
                return BadRequest();
            }

            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new InstallationUploadedFileRealatedQuery()
                {
                    tki_number = tki_number,
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

