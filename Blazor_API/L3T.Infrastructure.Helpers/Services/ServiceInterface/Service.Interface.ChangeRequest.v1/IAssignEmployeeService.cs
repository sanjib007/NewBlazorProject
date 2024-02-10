using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface IAssignEmployeeService
    {
        Task<ApiResponse> AddAssignEmployee(AddAssignEmployeeReq requestModel,string getUserid, string ip);
        Task<ApiResponse> UpdateAssignEmployee(UpdateAssignEmployeeReq ReqModel, string getUserid, string ip);
        Task<ApiResponse> GetAllAssignEmployeeQuery(AssignEmployeeListReqModel FilterReqModel,string route, string getUserid,string ip);
        Task<ApiResponse> GetAssignEmployeeByCrId(long CrId, string getUserid, string ip);
        Task<ApiResponse> AssignEmployeeDelete(long Id, string getUserid, string ip);
        Task<ApiResponse> GetAllAssignDeveloper(string userId, string ip);
        Task<ApiResponse> UpdateAllAssignDeveloper(string userId, string ip);
        Task<ApiResponse> UpdateAssignEmployeeTaskStatusIsCompleteOrIncomplete(long crId, string getUserid, string ip);
    }
}
