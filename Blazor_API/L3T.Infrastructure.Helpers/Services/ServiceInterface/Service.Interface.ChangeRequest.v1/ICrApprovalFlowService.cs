using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface ICrApprovalFlowService
    {
        Task<ApiResponse> AddCrApprovalFlow(AddCrApprovalFlowReq requestModel, string userId, string ip);
        Task<ApiResponse> UpdateCrApprovalFlow(UpdateCrApprovalFlowReq ReqModel, string userId, string ip);
        Task<ApiResponse> GetCrApprovalFlow(long CrId, string userId, string ip);
        Task<ApiResponse> CrApprovalFlowDelete(long Id, string userId, string ip);
        Task<ApiResponse> AddRemark(AddRemarkRequestModel ReqModel, string userId, string ip);
        Task<ApiResponse> OnlyApprovedDataByCrId(long CrId, string userId, string ip);


        Task<ApiResponse> CrApprovedApproverList(long Id,string userId, string ip);

    }
}
