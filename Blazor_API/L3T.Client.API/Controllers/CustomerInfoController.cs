using L3T.ChangeRequest.API.Controllers;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Repositories.Interface.Client;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Client;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.Client.API.Controllers
{
    [ApiController]
    [Route(CommonHelper.ControllerRoute)]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class CustomerInfoController : CustomsBaseController
    {
        private readonly IMISCustomerInformationService _misCustomerService;

        public CustomerInfoController(IMISCustomerInformationService misCustomerService)
        {
            _misCustomerService = misCustomerService;
        }


        [HttpGet("CustomerID/{mobileNo}")]
        public async Task<IActionResult> GetCustomerId(string mobileNo)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _misCustomerService.MisCustomerCode(mobileNo, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("CustomerPhoneNumber/{customerId}")]
        public async Task<IActionResult> GetCustomerMobileNo(string customerId)
        {
            var ip = await GetClientIPAddress();
            var getUserid = User.GetClaimUserId();
            var result = await _misCustomerService.MisCustomerPhone(customerId, getUserid, ip);
            return await responseCheck(result);
        }
        

    }
}
