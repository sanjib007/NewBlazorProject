using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tik4net.Objects.User;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IRsmChecklistService
    {
        Task<ApiResponse> AllDataForRSMCheckList(string clientID, string ip, string userId);
        Task<ApiResponse> GetChecklistData(string ip, string userId);
        Task<ApiResponse> GetRouterTypeData(string ip, string userId);
        Task<ApiResponse> GetControllerOwnerData(string ip, string userId);
        Task<ApiResponse> GetSingleApData(string ip, string userId);
        Task<ApiResponse> GetMultipleApData(string ip, string userId);
        Task<ApiResponse> GetChannelWidth20MHzData(string ip,string userId);
        Task<ApiResponse> GetGhzEnabledData(string ip, string userId);
        Task<ApiResponse> GetChannelWidthAutoData(string ip, string userId);
        Task<ApiResponse> GetChannelbetween149_161Data(string ip, string userId);
        Task<ApiResponse> GetRsmChecklistDetailsByClientId(string clientId, string ip, string userId);
        Task<ApiResponse> SaveRsmChecklistData(RsmCheckListRequestModel model,string userId, string ip);
    }
}
