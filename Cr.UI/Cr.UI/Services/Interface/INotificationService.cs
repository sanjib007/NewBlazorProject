using Cr.UI.Data.ChangeRequirementModel;
using Cr.UI.Data;

namespace Cr.UI.Services.Interface
{
    public interface INotificationService
    {
        Task<ApiResponse<PaginationModel<List<NotificationDetailsResponseModel>>>> GetAllNotification(string requestUri);
        Task<ApiResponse> NotificationUnreadToRead(string requestUri);
    }
}
