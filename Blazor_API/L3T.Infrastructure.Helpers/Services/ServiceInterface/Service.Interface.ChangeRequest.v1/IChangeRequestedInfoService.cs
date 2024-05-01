using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface IChangeRequestedInfoService
    {
        Task<string> testMethod(string l3Id);
        Task<ApiResponse> GetAllChangeRequest(ChangeRequestListReqModel model, string route, string userId, string ip);
        Task<ApiResponse> GetPendingAllChangeRequest(ChangeRequestListReqModel model, string route, string userId, string ip);
        Task<ApiResponse> GetStatusWiseTotalCR(string showFor, string department, string userId, string ip);
        Task<ApiResponse> AddChangeRequest(string userId,string ip);    
        Task<List<GetAllTotalCrByCatagoryWise>> GetAllCrTotalByCategory(string userId, string ip);
        Task<List<ChangeRequestedInfo>> GetLastFiveCr(string userId, string ip);
        Task<ApiResponse> GetAllFiles(long crid, string userId, string ip);
        Task<ApiResponse> CrUserPersonalStatus(ChangeRequestListReqModel model, string route, string userId, string ip);
        Task<ApiResponse> CrForDeveloperWise(ChangeRequestListReqModel model, string route, string userId, string ip);
    }
}
