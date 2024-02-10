using Cr.UI.Data;
using Cr.UI.Data.Socket;

namespace Cr.UI.Services.Interface
{
    public interface ISocketService
    {
        Task<ApiResponse> CallChartEndpoint(string requestUri, GetUserInfoFromMikrotikRequestModel model);
    }
}
