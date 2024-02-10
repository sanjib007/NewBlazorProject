using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IChecklistService
    {
        Task<ApiResponse> GetMisChecklistDetailsByTicketId(string ticketid, string ip);
        Task<ApiResponse> GetCustomerInfoByTicketId(string ticketid, string ip);
        Task<ApiResponse> GetChecklistData(string ip);
        Task<ApiResponse> GetRouterTypeData(string ip);
        Task<ApiResponse> GetControllerOwnerData(string ip);
        Task<ApiResponse> GetSingleApData(string ip);
        Task<ApiResponse> GetMultipleApData(string ip);
        Task<ApiResponse> GetChannelWidth20MHzData(string ip);
        Task<ApiResponse> GetGhzEnabledData(string ip);
        Task<ApiResponse> GetChannelWidthAutoData(string ip);
        Task<ApiResponse> GetChannelbetween149_161Data(string ip);
        Task<ApiResponse> SaveChecklistData(MisCheckListRequestModel model, string ip);
        Task<ApiResponse> UploadAndSaveChecklistFile(FileUploadModel uploadModel, ClaimsPrincipal user, string ip);
        Task<ApiResponse> GetMisChecklistDetailsB2BByTicketId(string ticketid, string ip);
        Task<ApiResponse> GetChecklistB2BData(string ip);
        Task<ApiResponse> SaveChecklistB2BData(MisCheckListRequestModel model, string ip);
    }
}
