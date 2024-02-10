using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Data;
using tik4net.Objects.User;

namespace L3T.FieldForceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class MisInstallationTicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MisInstallationTicketController(IMediator mediator)
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

        [HttpGet("GetSubscriptionInfo/{ticketId}")]
        public async Task<IActionResult> GetSubscriptionInfoByTicketId(string? ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var userId = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetSubscriptionInfoByTicketIdQuery() { ticketId = ticketId, userId = userId, ip = ip });
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

        [HttpGet("GetHardwareInfo/{ticketId}")]
        public async Task<IActionResult> GetHardwareInfoByTicketId(string? ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetHardwareInfoByTicketIdQuery() { ticketId = ticketId, ip = ip });
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

        [HttpGet("GetInternetInfo/{ticketId}")]
        public async Task<IActionResult> GetInternetInfoByTicketId(string? ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetInternetInfoByTicketIdQuery() { ticketId = ticketId, ip = ip });
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

        [HttpGet("GetIpTelephonyInfo/{ticketId}")]
        public async Task<IActionResult> GetIpTelephonyInfoByTicketId(string? ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetIpTelephonyInfoByTicketIdQuery() { ticketId = ticketId, ip = ip });
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

        [HttpGet("GetMktAndBillingInfo/{ticketId}")]
        public async Task<IActionResult> GetMktAndBillingInfoByTicketId(string? ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetMktAndBillingInfoByTicketIdQuery() { ticketId = ticketId, ip = ip });
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


        [HttpGet("GetAllTeamName/{ticketId}")]
        public async Task<IActionResult> GetAllTeamName(string? ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetTeamNameByTicketIdQuery() { userId = getUserid, ticketId = ticketId, ip = ip });
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

        [HttpPost("UpdateInstallationSchedule")]
        public async Task<IActionResult> UpdateInstallationSchedule(installationScheduleRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new UpdateInstallationScheduleDataCommand() { userId = getUserid, model = model, ip = ip });
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

        [HttpGet("PendingCategoryList")]
        public async Task<IActionResult> PendingCategoryList()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetPendingCategoryQuery() { ip = ip });
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


        [HttpGet("PendingReasonList/{categoryId}")]
        public async Task<IActionResult> PendingReasonList(string? categoryId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetPendingReasonQuery() { categoryId = categoryId, ip = ip });
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

        [HttpGet("GetServiceCheckboxList/{ticketId}")]
        public async Task<IActionResult> GetServiceCheckboxList(string? ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetCheckboxListQuery() { ticketId = ticketId, ip = ip });
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



        [HttpPost("SendMailAndUpdateComment")]
        public async Task<IActionResult> SendMailAndUpdateComment(UpdateCommentRequestModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new UpdateCommentQuery() { model = model, user = user, ip = ip });
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


        [HttpPost("SendSMS")]
        public async Task<IActionResult> SendSMSToCustomer(SendSmsRequestModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new SendSmsToCustomerQuery() { model = model, user = user, ip = ip });
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


        [HttpGet("GetInstallationComment/{ticketId}")]
        public async Task<IActionResult> GetInstallationComment(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetInstallationCommentQuery() { ticketId = ticketId, ip = ip });
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


        [HttpGet("GetGeneralInfo/{ticketId}")]
        public async Task<IActionResult> GetGeneralInfo(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GeneralInfoQuery() { ticketId = ticketId, ip = ip });
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


        [HttpPost("DoneGeneralInfo")]
        public async Task<IActionResult> DoneGeneralInfo(GeneralInfoDoneRequestModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new DoneGeneralInfoCommand() { model = model, user = user, ip = ip });
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

        [HttpPost("UpdateGeneralInfo")]
        public async Task<IActionResult> UpdateGeneralInfo(GeneralInfoUpdateModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new UpdateGeneralInfoCommand() { model = model, user = user, ip = ip });
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

        [HttpGet("GetIpTelephonyEditInfo/{ticketId}")]
        public async Task<IActionResult> GetIpTelephonyEditInfo(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetIpTelephonyInfoQuery() { ticketId = ticketId, ip = ip });
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

        [HttpPost("DoneIpTelephonyInfo")]
        public async Task<IActionResult> DoneIpTelephonyInfo(IpTelephonyDoneRequestModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new DoneIpTelephonyInfoCommand() { model = model, user = user, ip = ip });
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

        [HttpPost("UpdateIpTelephonyInfo")]
        public async Task<IActionResult> UpdateIpTelephonyInfo(IpTelephonyUpdateRequestModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new UpdateIpTelephonyInfoCommand() { model = model, user = user, ip = ip });
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

        [HttpPost("DoneHardwareInfo")]
        public async Task<IActionResult> DoneHardwareInfo(HardwareInfoDoneRequestModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new DoneHardwareInfoCommand() { model = model, user = user, ip = ip });
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


        [HttpPost("UpdateHardwareInfo")]
        public async Task<IActionResult> UpdateHardwareInfo(HardwareInfoUpdateRequestModel model)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new UpdateHardwareInfoCommand() { model = model, user = user, ip = ip });
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
		
		

        [HttpGet("GetIntranetInfo/{ticketId}")]
        public async Task<IActionResult> GetIntranetInfo(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetIntranetInfoQuery() { ticketId = ticketId, ip = ip });
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

        [HttpPost("SaveComment")]
        public async Task<IActionResult> SaveComment(MisInstallationTickeAddCommentRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                model.UserId = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                model.UserEmail = User.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
                model.UserCellNo = User.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();
                model.UserName = User.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                model.DesignationName = User.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                model.DepartmentName = User.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                model.Ip = ip;

                var meidatorObj = await _mediator.Send(new MisInstallationTicketAddCommentCommand() { model = model});
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
