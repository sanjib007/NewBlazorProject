using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace L3T.ChangeRequest.API.Controllers
{
    //[ApiController]
    //[Route(CommonHelper.ControllerRoute)]
    [Controller]
    public class CustomsBaseController : ControllerBase
    {
        private IBaseControllerCommonService _cRMenuAndPermissionSetupService;
        protected IBaseControllerCommonService cRMenuAndPermissionSetupService => _cRMenuAndPermissionSetupService ?? (_cRMenuAndPermissionSetupService = HttpContext.RequestServices.GetService<IBaseControllerCommonService>());
        protected CustomsBaseController(IBaseControllerCommonService cRMenuAndPermissionSetupServiceAdd)
        {
            _cRMenuAndPermissionSetupService = cRMenuAndPermissionSetupServiceAdd;
        }

        protected async Task<string> GetClientIPAddress()
        {
            //await _cRMenuAndPermissionSetupService.InsertMenuSetupTable(HttpContext);

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
