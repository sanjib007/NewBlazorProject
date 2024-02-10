using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.Cams
{
    public interface ISocketService
    {
        Task<ApiResponse> GetLiveDataFormMikrotikRouter(GetUserInfoFromMikrotikRequestModel requestModel);

         
    }
}
