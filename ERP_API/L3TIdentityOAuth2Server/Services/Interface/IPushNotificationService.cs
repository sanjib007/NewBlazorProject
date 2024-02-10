using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;

namespace L3TIdentityOAuth2Server.Services.Interface
{
    public interface IPushNotificationService
    {
        Task<PushNotificationResponseModel> SendPushNotificationAsync(SendPushNotificationModel model,
            string requiestedByUserId);
    }
}
