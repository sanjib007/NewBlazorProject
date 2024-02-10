using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface ITempChangeRequestedInfoService
    {
        Task<ApiResponse> AddChangeRequirement(AddTempChangeRequestModel requestModel, string userId, string ip);
        Task<ApiResponse> UpdateChangeRequirement(AddTempChangeRequestModel ReqModel, string userId, string ip);
        Task<ApiResponse> GetChangeRequirementById(long Id, string userId, string ip);
        Task<ApiResponse> ChangeRequirementDelete(long Id, string userId, string ip);
        Task<ApiResponse> UncompleteChangeRequest(string userId, string ip);
        Task<ApiResponse> RemovedFile(RemovedFileRequestModel model, string userId, string ip);
    }
}
