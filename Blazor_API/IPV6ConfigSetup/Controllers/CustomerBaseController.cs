using IPV6ConfigSetup.DataAccess.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace IPV6ConfigSetup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerBaseController : ControllerBase
    {
        protected async Task<string> GetClientIPAddress()
        {
            var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var localIpAddress = httpConnectionFeature?.LocalIpAddress;
            var remoteIpAddress = httpConnectionFeature?.RemoteIpAddress;
            var ip = localIpAddress.ToString() + "/" + remoteIpAddress.ToString();
            return ip;
        }

        protected async Task<IActionResult> responseCheck(ApiResponse response)
        {
            if (response.Status == "Error" && response.StatusCode == 400)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = response.Message
                });
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
