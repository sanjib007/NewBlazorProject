using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess.Model;

namespace L3T.OAuth2DotNet7.Services.Interface
{
    public interface IIdentityReauestResponseService
    {
        Task<ApiResponse> CreateResponseRequest(object request, object response, string cusIp, string methodName, string userId, string successMessage = null, string errorMessage = null);
        Task<bool> IsHaveEncryptionResult(RSAEncryptDataDuplocationCheckModel request);
        Task<ApiResponse> AddRSAEncryptDataForDuplicationCheck(RSAEncryptDataDuplocationCheckModel request);
    }
}
