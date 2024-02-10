using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using System.Security.Claims;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IForwardTicketService
    {
        Task<ApiResponse> ForwardTicketDetails(string ticketId, string ip, string userId);
        Task<ApiResponse> Category(string ip, string userId);
        Task<ApiResponse> FowardToList(string ip, string userId);
        Task<ApiResponse> ForwardTicket(ForwardTicketRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> ComplainInformation(string ticketId, ClaimsPrincipal user, string ip);
        Task<ApiResponse> PushNotificationForTicketAssignOrForward(PushNotificationRequestModel requestModel, string Ip);

    }
}
