using CorePush.Google;
using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.NotificationRelatedModels;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;
using L3TIdentityOAuth2Server.Services.Interface;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using static L3TIdentityOAuth2Server.DataAccess.RequestModel.GoogleNotification;

namespace L3TIdentityOAuth2Server.Services.Implementation
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly IIdentityRequestResponseService _identityRequestResponseService;

        public PushNotificationService(IOptions<FcmNotificationSetting> settings, IIdentityRequestResponseService identityRequestResponseService)
        {
            _fcmNotificationSetting = settings.Value;
            _identityRequestResponseService = identityRequestResponseService;
        }

        public async Task<PushNotificationResponseModel> SendPushNotificationAsync(SendPushNotificationModel notificationModel,
            string requiestedByUserId)
        {
            var methodName = "PushNotificationService/SendPushNotificationAsync";
            PushNotificationResponseModel response = new PushNotificationResponseModel();
            try
            {
                if (notificationModel.IsAndroiodDevice)
                {
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    };
                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = notificationModel.DeviceId;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);
                    var fcmResponse = ConvertObjecttoString(fcmSendResponse);



                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        var identityRequestResponseModel = new IdentityRequestResponseModel()
                        {
                            NotificationType = "PushNotification",
                            RequestText = dataPayload.Title + "--" + dataPayload.Body,
                            ResponseText = fcmResponse,
                            UserId = notificationModel.UserId,
                            ErrorLog = fcmSendResponse.Failure.ToString(),
                            RequestedIP = notificationModel.RequestedIP,
                            CreatedAt = DateTime.Now,
                            MethodName = methodName
                        };

                        await _identityRequestResponseService.AddIdentityRequestResponseforPushNotificationAsync(identityRequestResponseModel);
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        var identityRequestResponseModel = new IdentityRequestResponseModel()
                        {
                            NotificationType = "PushNotification",
                            RequestText = dataPayload.Title + "--" + dataPayload.Body,
                            ResponseText = fcmResponse,
                            UserId = notificationModel.UserId,
                            ErrorLog = fcmSendResponse.Failure.ToString(),
                            RequestedIP = notificationModel.RequestedIP,
                            CreatedAt = DateTime.Now,
                            MethodName = methodName
                        };
                        await _identityRequestResponseService.AddIdentityRequestResponseforPushNotificationAsync(identityRequestResponseModel);
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }

        public string ConvertObjecttoString(object atype)
        {
            StringBuilder sb = new StringBuilder();
            //if (atype == null) return new Dictionary<string, object>();
            if (atype == null)
            {
                return null;
            }
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            // Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                //dict.Add(prp.Name, value);
                sb.AppendFormat("{0}: {1};", prp.Name, value);
            }
            return sb.ToString();
        }
    }
}
