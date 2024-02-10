using AutoMapper;
using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess;
using L3TIdentityOAuth2Server.DataAccess.IdentityModels;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;
using L3TIdentityOAuth2Server.Pagination;
using L3TIdentityOAuth2Server.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace L3TIdentityOAuth2Server.Services.Implementation
{
    public class NotificationSendService : INotificationSendService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly IdentityTokenServerDBContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IPushNotificationService _pushNotificationService;

        public NotificationSendService(IdentityTokenServerDBContext context, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, RoleManager<AppRoles> roleManager,
            IUriService uriService, IMapper mapper, IPushNotificationService pushNotificationService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _uriService = uriService;
            _mapper = mapper;
            _pushNotificationService = pushNotificationService;
        }

        public async Task<ApiResponse> SendPushNotification(PushNotificationRequestModel requestModel, string ip)
        {
            var methodName = "NotificationSendService/SendPushNotification";
            try
            {
                string[] employeeList = requestModel.AssignedPersonsEmployeeId;
                for (int i = 0; i < employeeList.Length; i++)
                {

                    var assignedPersonUserId = employeeList[i].ToString();
                    var user = await _userManager.FindByNameAsync(assignedPersonUserId);
                    if (user != null)
                    {
                        var deviceId = user.DeviceId;

                        var pushNotificationSendModel = new SendPushNotificationModel()
                        {
                            DeviceId = deviceId,   // user.deviceid
                            IsAndroiodDevice = true,
                            Title = requestModel.SubjectOrSuspectedCase,
                            Body = requestModel.Description,
                            RequestedIP = ip,
                            UserId = assignedPersonUserId
                        };

                        var result = await _pushNotificationService.SendPushNotificationAsync(pushNotificationSendModel,
                            requestModel.RequiestedByUserId);
                    }


                }
                return new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Push-Notification Send process excuted."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                };
            }
        }
    }
}
