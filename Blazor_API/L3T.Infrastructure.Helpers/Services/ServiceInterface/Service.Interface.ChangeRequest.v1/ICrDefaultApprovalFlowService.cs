using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface ICrDefaultApprovalFlowService
    {
        Task<ApiResponse> AddCrDefaultApprovalFlow(AddCrDefaultApprovalFlowReq requestModel, string userId, string ip);
        Task<ApiResponse> UpdateCrDefaultApprovalFlow(UpdateCrDefaultApprovalFlowReq ReqModel, string userId, string ip);
        Task<ApiResponse> GetAllDefaultApprovalFlow(string userId, string ip);
        Task<ApiResponse> CrDefaultApprovalFlowDelete(long Id, string userId, string ip);
        Task<ApiResponse> ActiveInactiveApprover(UpdateCrDefaultApprovalFlowReq ReqModel, string userId, string ip);


    }
}
