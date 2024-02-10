using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface IChangeRequestLogService
    {
        Task<ApiResponse> AddChangeRequestLog(AddChangeRequestLogReq requestModel, string getUserid, string ip);
    }
}
