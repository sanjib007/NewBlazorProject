using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;

namespace L3TIdentityOAuth2Server.Services.Interface
{
    public interface INotificationSendService
    {
        Task<ApiResponse> SendPushNotification(PushNotificationRequestModel requestModel, string ip);
    }
}
