using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels.RSM;
using System.Security.Claims;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IRsmInstallationTicketService
    {
        Task<ApiResponse> GetSalesPersonsInfo(ClaimsPrincipal user, string ip, string employeeId);
        Task<ApiResponse> AddFonocInfo(RSMfonocAddRequestModel model, string user, string ip);

        Task<ApiResponse> UpdateFonocInfo(RSMfonocUpdateRequestModel model, string user, string ip);

        Task<ApiResponse> UpdateFonocRouterInfo(RSMfonocRouterUpdateRequestModel model, string user, string ip);

        Task<ApiResponse> InstallationFileUpload(RSMInstallationFileUploadRequestModel model, string user, string ip);

        Task<ApiResponse> GetInstallationInfoData(string userId, string ip, string ticketId);

        Task<ApiResponse> GetP2mHomeSCRIDInfo(string user, string ip, string prefixText);

        Task<ApiResponse> GetSummitLinkIDInfo(string user, string ip, string prefixText);

        Task<ApiResponse> GetPendingInstallation(string user, string ip);

        Task<ApiResponse> AddCommentData(string userId, string ip, RsmInstallationAddCommentsRequestModel model);
        Task<ApiResponse> RouterModelData(string userId, string ip, int brandId);
        Task<ApiResponse> SheduleDataUpdate(string userId, string ip, InstallationSheduleUpdateRequestModel model);
        Task<ApiResponse> GetAddColorInfoData(string ticketId, string userId, string requestedIp);
        Task<ApiResponse> AddColorData(string userId, string ip, AddColorRequestModel model);
        Task<ApiResponse> UpdateColorData(string userId, string ip, UpdateColorRequestModel model);
        Task<ApiResponse> NetworkConnectionDoneP2M(NetworkConnectionRequestModel model, string userId, string ip);
        Task<ApiResponse> NetworkConnectionUpdateP2M(NetworkConnectionRequestModel model, string userId, string ip);
    }
}
