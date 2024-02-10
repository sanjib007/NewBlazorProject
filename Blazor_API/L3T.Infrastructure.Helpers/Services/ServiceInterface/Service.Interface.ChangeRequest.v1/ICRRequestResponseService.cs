using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface ICRRequestResponseService
    {
        Task<ApiResponse> CRRequestResponseList(NotificationListFilterReqModel notificationListFilterReqModel, string getUserid, string route);
        Task<ApiResponse> AddCRRequestResponse(CRRequestResponseModel CRRequestResponse);
        Task<ApiResponse> CreateResponseRequest(object request, object response, string cusIp, string methodName, string userId, string successMess, string errorMessage = null);

		//Task<ApiResponse> UpdateCRRequestResponse(CRRequestResponseModel CRRequestResponse, string getUserid);
		//Task<ApiResponse> DeleteCRRequestResponse(long id);

	}
}
