using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess.NotificationRelatedModels;

namespace L3TIdentityOAuth2Server.Services.Interface
{
    public interface IIdentityRequestResponseService
    {
        Task<ApiSuccess> AddIdentityRequestResponseforPushNotificationAsync(IdentityRequestResponseModel sendPushNotificationModel);
    }
}
