using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using L3T.Utility.Helper;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;

namespace L3T.ChangeRequest.API.Controllers
{
    [Route(CommonHelper.CrApiControllerRoute)]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = CommonHelper.AllRole)]

    public class NotificationDetailsController : CustomsBaseController
    {
        private readonly INotificationDetailsService _iNotificationDetailsService;

        public NotificationDetailsController(
            IBaseControllerCommonService cRMenuAndPermissionSetupService, 
            INotificationDetailsService iNotificationDetailsService
            ) : base(cRMenuAndPermissionSetupService)
        {
            _iNotificationDetailsService = iNotificationDetailsService;
        }
     
        [HttpGet("NotificationDetailsList/{pageNumber}")]
        public async Task<IActionResult> NotificationDetailsList(int pageNumber)
        {
            var getUserid = User.GetClaimUserId();
            var ip = await GetClientIPAddress();
            var route = Request.Path.Value;
            NotificationListFilterReqModel notificationListFilterReqModel = new NotificationListFilterReqModel();
            notificationListFilterReqModel.ApproverEmpId = getUserid;
            notificationListFilterReqModel.PageNumber = pageNumber;
            notificationListFilterReqModel.PageSize = 5;
            var result = await _iNotificationDetailsService.NotificationDetailsList(notificationListFilterReqModel, getUserid, route, ip);
            return await responseCheck(result);
        }

        [AllowAnonymous]
        [HttpPost("AddNotificationDetails")]
        public async Task<IActionResult> AddNotificationDetails(NotificationDetails notificationDetails)
        {
            var getUserid = User.GetClaimUserId();
            var ip = await GetClientIPAddress();
            var result = await _iNotificationDetailsService.AddNotificationDetails(notificationDetails, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpPost("UpdateNotificationDetails")]
        public async Task<IActionResult> UpdateNotificationDetails(NotificationDetails notificationDetails)
        {
            var getUserid = User.GetClaimUserId();
            var ip = await GetClientIPAddress();
            var result = await _iNotificationDetailsService.UpdateNotificationDetails(notificationDetails, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpDelete("DeleteNotificationDetails/{id}")]
        public async Task<IActionResult> DeleteNotificationDetails(long id)
        {
            var getUserid = User.GetClaimUserId();
            var ip = await GetClientIPAddress();
            var result = await _iNotificationDetailsService.DeleteNotificationDetails(id, getUserid, ip);
            return await responseCheck(result);
        }

        [HttpGet("NotificationUnreadToRead")]
        public async Task<IActionResult> NotificationUnreadToRead()
        {
            var getUserid = User.GetClaimUserId();
            var ip = await GetClientIPAddress();
            var result = await _iNotificationDetailsService.UpdateNotificationUnreadToRead(getUserid, ip);
            return await responseCheck(result);
        }




    }
}
