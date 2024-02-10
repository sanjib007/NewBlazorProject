using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;
using L3TIdentityOAuth2Server.Services.Interface;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace L3TIdentityOAuth2Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendNotificationController : ControllerBase
    {
        private readonly INotificationSendService _notificationService;

        public SendNotificationController(INotificationSendService notificationService)
        {
            _notificationService = notificationService;
        }

        protected async Task<string> GetClientIPAddress()
        {
            var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var localIpAddress = httpConnectionFeature?.LocalIpAddress;
            //var localIpAddress = httpConnectionFeature?.RemoteIpAddress;
            var ip = localIpAddress.ToString();
            return ip;
        }

        [HttpPost("SendPushNotification")]
        public async Task<IActionResult> SendPushNotification(PushNotificationRequestModel request)
        {
            try
            {
                string[]? assignedEmployeeList = request.AssignedPersonsEmployeeId;
                if (assignedEmployeeList.Length == 0)
                {
                    return BadRequest(new ApiResponse()
                    {
                        Status = "Error",
                        StatusCode = 400,
                        Message = "Assigned or Forwarded Employee List is Empty"
                    });
                }
                var user = User;
                var ip = await GetClientIPAddress();
                var response = await _notificationService.SendPushNotification(request, ip);


                return Ok(response);
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
