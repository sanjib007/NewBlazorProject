

namespace MicrotikBlazorWA.Socket
{
    public interface ISocketService
    {
        Task<ApiResponse> CallChartEndpoint(string requestUri, GetUserInfoFromMikrotikRequestModel model);
    }
}
