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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class RestController : Controller
    {
        private readonly IMediator _mediator;
        public RestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<string> GetClientIPAddress()
        {
            var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var localIpAddress = httpConnectionFeature?.LocalIpAddress;
            var remoteIpAddress = httpConnectionFeature?.RemoteIpAddress;
            var ip = localIpAddress.ToString() +"/"+ remoteIpAddress.ToString();
            return ip;
        }

        //[AllowAnonymous]
        //[HttpGet("testdate")]
        //public async Task checkDate()
        //{
        //    var a = Convert.ToDateTime("2023-01-18 15:21:09") - Convert.ToDateTime("1970-01-01 00:00:00");
        //    var second = a.TotalSeconds;

        //    TimeSpan time = TimeSpan.FromSeconds(1674040977);
        //    DateTime dateTime = Convert.ToDateTime("1970-01-01 00:00:00").Add(time);

        //    //var b = Convert.ToDateTime("1970-01-01 00:00:00")
        //}

        [AllowAnonymous]
        [HttpGet("AddCoordinates")]
        public async Task<IActionResult> Index([FromQuery] AddCoordinatesRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new AddCoordinatesCommand() { model = model, ip = ip });
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
        [HttpGet("GetATicket/{ticketId}")]
        public async Task<IActionResult> GetAllTicket(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetATicektByTicketIdQuery() { ticketId = ticketId, userid= "Web", ip = ip });
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
        [HttpGet("GetLocationForAllUser")]
        public async Task<IActionResult> GetLocationForAllUser([FromQuery] string date, string? fromTime, string? toTime)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var user = User;
                var meidatorObj = await _mediator.Send(new GetLocationForAllUserQuery() { Date = date, formTime = fromTime, toTime = toTime, ip = ip });
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
        [HttpGet("GetLocationForAUser")]
        public async Task<IActionResult> GetLocationForAUser([FromQuery] string userId, string date, string? fromTime, string? toTime)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var user = User;
                var meidatorObj = await _mediator.Send(new GetLocationForAUserQuery() { userId = userId, Date = date, FromTime = fromTime, ToTime = toTime, ip = ip });
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

        [HttpGet("GetAllTicket")]
        public async Task<IActionResult> GetAllTicket()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetAllPendingTicketQuery() { userId = getUserid, ip = ip });
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
        [HttpGet("GetAllTicketByEmpId")]
        public async Task<IActionResult> GetAllTicketByEmployeeId([FromQuery] AllTicketListAndCountRequestModel model)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new GetAllTicketListAndCountQuery() { userId = model.EmpId, ip = ip, lastDays = model.LastDays });
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

        [HttpGet("GetTicketLog/{ticketId}")]
        public async Task<IActionResult> GetTicketLog(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetTicketLogQuery() { TicketId = ticketId, Ip = ip, UserId = getUserid });
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

        [HttpGet("GetClosingNatureList")]
        public async Task<IActionResult> GetClosingNatureList()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetClosingNatureQuery() { ip = ip, UserId = getUserid });
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

        [HttpGet("GetReasonForOutageList/{id}")]
        public async Task<IActionResult> GetReasonForOutageList(long id)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetReasonForOutageQuery() { closingNatureId = id, ip = ip, UserId = getUserid });

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

        [HttpGet("GetSupportDelayReasonList")]
        public async Task<IActionResult> GetSupportDelayReasonList()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetSupportDelayReasonQuery() { ip = ip, UserId = getUserid });
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

        [HttpGet("GetSupportType")]
        public async Task<IActionResult> GetSupportType()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new GetSupportTypeQuery() { ip = ip, UserId = getUserid });
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

        [HttpGet("ChangeEngineerList/{ticketId}")]
        public async Task<IActionResult> ChangeEngineerList(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new ChangeEngineerListQuery() { ticketId = ticketId, ip = ip, UserId = getUserid });
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

        [HttpGet("InitialServiceRestoredNotification/{ticketId}")]
        public async Task<IActionResult> InitialServiceRestoredNotification(string ticketId)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new InitialServiceRestoredNotificationQuery() { ticketId = ticketId, user = user, ip = ip });
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

        [HttpGet("ResolvedDetailsMail/{ticketId}")]
        public async Task<IActionResult> ResolvedDetailsMail(string ticketId)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new ResolvedDetailsMailQuery() { ticketId = ticketId, user = user, ip = ip });
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

        [HttpPost("ResolvedTicket")]
        public async Task<IActionResult> ResolvedTicket(ResolvedTicketRequestModel request)
        {
            try
            {
                var user = User;
                var ip = await GetClientIPAddress();
                var meidatorObj = await _mediator.Send(new ResolvedTicketCommand() { model = request, user = user, ip = ip });
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

        [HttpGet("ForwardTicketDetail/{ticketId}")]
        public async Task<IActionResult> ForwardTicketDetail(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new ForwardTicketDetailQuery() { TicketId = ticketId, UserId = getUserid, ip = ip });
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

        [HttpGet("TicketCategory")]
        public async Task<IActionResult> TicketCategory()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new TicketCategoryQuery() { ip = ip, UserId = getUserid });
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

        [HttpGet("ForwardToList")]
        public async Task<IActionResult> ForwardToList()
        {
            try
            {
                var ip = await GetClientIPAddress();
                var getUserid = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
                var meidatorObj = await _mediator.Send(new ForwardToListQuery() { ip = ip, UserId = getUserid });
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

        [HttpPost("ForwardTicket")]
        public async Task<IActionResult> ForwardTicket(ForwardTicketRequestModel request)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var user = User;
                var meidatorObj = await _mediator.Send(new ForwardTicketCommand() { model = request, user = user, ip = ip });
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

        [HttpGet("ComplainTicketInfo/{ticketId}")]
        public async Task<IActionResult> ComplainTicketInfo(string ticketId)
        {
            try
            {
                var ip = await GetClientIPAddress();
                var user = User;
                var meidatorObj = await _mediator.Send(new ComplainInformationQuery() { ticketId = ticketId, user = user, ip = ip });
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
