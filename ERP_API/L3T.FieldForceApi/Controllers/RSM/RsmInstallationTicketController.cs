using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationCommand;
using L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels.RSM;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Data;

namespace L3T.FieldForceApi.Controllers.RSM
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class RsmInstallationTicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RsmInstallationTicketController(IMediator mediator)
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

        [HttpGet("GetRsmInstallationInfo")]
        public async Task<IActionResult> GetRsmInstallationInfo(string ticketId)
        {
            var ip = await GetClientIPAddress();
            var userid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var meidatorObj = await _mediator.Send(new RsmInstallationInfoQuery()
                {
                    userId = userid,
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

        [HttpGet("GetAddColorInfo")]
        public async Task<IActionResult> GetAddColorInfo(string ticketId)
        {
            var ip = await GetClientIPAddress();
            var userid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var meidatorObj = await _mediator.Send(new GetAddColorInfoQuery()
                {
                    userId = userid,
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

        [HttpPost("AddColorInfo")]
        public async Task<IActionResult> AddColorInfo(AddColorRequestModel model)
        {
            var ip = await GetClientIPAddress();
            var userid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var meidatorObj = await _mediator.Send(new RsmInstallationAddColorCommand()
                {
                    userId = userid,
                    ip = ip,
                    model = model
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



        [HttpPost("UpdateColorInfo")]
        public async Task<IActionResult> UpdateColorInfo(UpdateColorRequestModel model)
        {
            var ip = await GetClientIPAddress();
            var userid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var meidatorObj = await _mediator.Send(new RsmInstallationUpdateColorCommand()
                {
                    userId = userid,
                    ip = ip,
                    model = model   
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


        [HttpPost("AddComments")]
        public async Task<IActionResult> AddComments(RsmInstallationAddCommentsRequestModel model)
        {
            var ip = await GetClientIPAddress();
            var userid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var meidatorObj = await _mediator.Send(new RsmInstallationAddCommentsCommand()
                {
                    userId = userid,
                    ip = ip,
                    model = model
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


        [HttpGet("RouterModel")]
        public async Task<IActionResult> RouterModel(int brandId)
        {
            var ip = await GetClientIPAddress();
            var userid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var meidatorObj = await _mediator.Send(new GetRouterModelListQuery()
                {
                    userId = userid,
                    ip = ip,
                    brandId= brandId
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


        [HttpPost("SheduleUpdate")]
        public async Task<IActionResult> SheduleUpdate(InstallationSheduleUpdateRequestModel model)
        {
            var ip = await GetClientIPAddress();
            var userid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var meidatorObj = await _mediator.Send(new InstallationSheduleUpdateCommand()
                {
                    userId = userid,
                    ip = ip,
                    model = model
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



        [HttpGet("GetPersonalInfo")]
        public async Task<IActionResult> GetPersonalInfo(string clientId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new SalesPersonsInfoQuery()
                {
                    user = user,
                    ip = ip,
                    employeeId = clientId
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
       

        [HttpPost("UploadRsmInstalltionRelatedFile")]
        public async Task<ActionResult> UploadRsmInstalltionRelatedFile(string ticketId,
            [FromForm] FileUploadModel fileDetails, string fileTitle, string clientCode,
            string uploadByUserId, string remarks
             )
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                var user = User;
                //var ip = await GetClientIPAddress();
                //var meidatorObj = await _mediator.Send(new RsmInstallationFileUploadCommand()
                //{
                //    fileDetails = fileDetails,
                //    user = user,
                //    ip = ip,
                //    file_header = file_header,
                //    cli_code = cli_code,
                //    code_sl = code_sl,
                //    file_categry_tki_number = file_categry_tki_number,
                //    upload_by = upload_by,
                //    ServiceID = ServiceID
                //});
                //return Ok(meidatorObj);
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }



        [HttpPost("AddFonoc")]
        public async Task<IActionResult> Insertfonoc(RSMfonocAddRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault(); 
                var meidatorObj = await _mediator.Send(new FonocInstallationCreateCommand() { model = model, userId = getUserid, ip = ip });
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

        [HttpPost("UpdateFonoc")]
        public async Task<IActionResult> Updatefonoc(RSMfonocUpdateRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new FonocInstallationUpdateCommand() { model = model, userId = getUserid, ip = ip });
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

        [HttpPost("UpdateFonocRouter")]
        public async Task<IActionResult> UpdateFonocRouter(RSMfonocRouterUpdateRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new FonocInstallationRouterUpdateCommand() { model = model, userId = getUserid, ip = ip });
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


        [HttpPost("UploadAttachFile")]
        public async Task<ActionResult> UploadAttachFile([FromForm] RSMInstallationFileUploadRequestModel model)
        {
           

            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new InstallationUploadAttachFileCommand() { model = model, userId = getUserid, ip = ip });

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


        [HttpGet("P2mHomeSCRIDList")]
        public async Task<ActionResult> GetP2mHomeSCRIDList(string prefixText)
        {


            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new P2mHomeScridInfoQuery() { prefixText = prefixText, userId = getUserid, ip = ip });

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

        [HttpGet("SummitLinkIDList")]
        public async Task<ActionResult> GetSummitLinkIDList(string prefixText)
        {


            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new SummitLinkidInfoQuery() { prefixText = prefixText, userId = getUserid, ip = ip });

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


        [HttpGet("PendingInstallationList")]
        public async Task<ActionResult> GetPendingInstallationList()
        {


            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new PendingInstallationInfoQuery() { userId = getUserid, ip = ip });

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

        [HttpPost("NetworkConnectionDoneP2M")]
        public async Task<IActionResult> NetworkConnectionDoneP2M(NetworkConnectionRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new RSMNetworkConnectionDoneCommand() { model = model, userId = getUserid, ip = ip });
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

        [HttpPost("NetworkConnectionUpdateP2M")]
        public async Task<IActionResult> NetworkConnectionUpdateP2M(NetworkConnectionRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new RSMNetworkConnectionUpdateCommand() { model = model, userId = getUserid, ip = ip });
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
