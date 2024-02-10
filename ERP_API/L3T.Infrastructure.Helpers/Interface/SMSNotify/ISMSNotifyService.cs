using L3T.Infrastructure.Helpers.CommonModel;

namespace L3T.Infrastructure.Helpers.Interface.SMSNotify;

public interface ISMSNotifyService
{
    Task<ApiResponse> GetSMSNotification();
    Task<ApiResponse> SmsPushInMySqlDB();
}