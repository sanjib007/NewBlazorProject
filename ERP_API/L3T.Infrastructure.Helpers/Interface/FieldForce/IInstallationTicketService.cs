using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IInstallationTicketService
    {
        Task<ApiResponse> GetSubscriptionInfoByTicketId(string ticketid, string userId, string ip);
        Task<ApiResponse> GetHardwareInfoByTicketId(string ticketid, string ip);
        Task<ApiResponse> GetInternetInfoByTicketId(string ticketid, string ip);
        Task<ApiResponse> GetIpTelephonyInfoByTicketId(string ticketid, string ip);
        Task<ApiResponse> GetMktAndBillingInfoByTicketId(string ticketid, string ip);
        Task<ApiResponse> GetAllTeamNameByTicketId(string userId, string ticketid, string ip);
        Task<ApiResponse> UpdateInstallationScheduleData(string userId, installationScheduleRequestModel model, string ip);
        Task<ApiResponse> GetPendingCategoryList(string ip);
        Task<ApiResponse> GetPendingReasonList(string categoryId, string ip);
        Task<ApiResponse> GetServiceCheckboxListData(string ticketId, string ip);
        Task<ApiResponse> UpdateCommentData(UpdateCommentRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> SendSmsText(SendSmsRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> GetInstallationCommentData(string ticketid, string ip);
        Task<ApiResponse> GetGenaralInfoData(string ticketid, string ip);
        Task<ApiResponse> DoneGeneralInfoData(GeneralInfoDoneRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> UpdateGeneralInfoData(GeneralInfoUpdateModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> GetIpTelephonyData(string ticketid, string ip);
        Task<ApiResponse> DoneIpTelephonyInfoData(IpTelephonyDoneRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> UpdateIpTelephonyInfoData(IpTelephonyUpdateRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> DoneHardwareInfoData(HardwareInfoDoneRequestModel model, ClaimsPrincipal user, string ip);
        Task<ApiResponse> UpdateHardwareInfoData(HardwareInfoUpdateRequestModel model, ClaimsPrincipal user, string ip);
		Task<ApiResponse> GetIntranetInfoData(string ticketid, string ip);
        Task<ApiResponse> SeveAddComment(MisInstallationTickeAddCommentRequestModel model);

    }
}