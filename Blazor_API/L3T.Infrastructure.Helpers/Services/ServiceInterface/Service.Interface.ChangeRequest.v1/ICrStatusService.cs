using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface ICrStatusService
    {
        Task<ApiResponse> AddStatus(AddStatusReq requestModel, string userId, string ip);
        Task<ApiResponse> UpdateStatus(UpdateStatusReq ReqModel, string userId, string ip);
        Task<ApiResponse> GetAllStatus( string userId, string ip);
        Task<ApiResponse> DeleteStatus(long Id, string userId, string ip);
        Task<ApiResponse> UpdateCRStatus(UpdateCrStatusRequestModel model, string userId, string ip);
        Task<ApiResponse> UpdateCRProcessStatus(UpdateCrStatusRequestModel model, string userId, string ip);
    }
}
