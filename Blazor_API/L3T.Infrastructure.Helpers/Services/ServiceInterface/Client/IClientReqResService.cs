using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface
{
    public interface IClientReqResService
    {
        //Task<ApiResponse> AddCRRequestResponse(IDRequestResponseModel IDRequestResponseModel);
        //Task<ApiResponse> BadRequest(string exMessage);
        Task<ApiResponse> CreateResponseRequest(object request, object response, string cusIp, string methodName, string userId, string successMess, string errorMessage = null);
        Task<ApiResponse> BadRequest(string exMessage);
        //Task<ApiResponse> UpdateAssignEmployee(UpdateAssignEmployeeReq ReqModel, string getUserid, string ip);
        //Task<ApiResponse> GetAllAssignEmployeeQuery(AssignEmployeeListReqModel FilterReqModel,string route, string getUserid,string ip);
        //Task<ApiResponse> GetAssignEmployeeByCrId(long CrId, string getUserid, string ip);
        //Task<ApiResponse> AssignEmployeeStatusChange(long Id, string getUserid, string ip);
        //Task<ApiResponse> GetAllAssignDeveloper();
    }
}
