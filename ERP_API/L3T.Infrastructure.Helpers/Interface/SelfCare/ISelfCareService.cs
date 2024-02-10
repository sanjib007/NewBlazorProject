using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.SelfCare.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.SelfCare
{
    public interface ISelfCareService
    {
        Task<ApiResponse> testMethod(int id);
        Task<ApiResponse> ServiceCreate(ServiceCreateRequestModel model, string ip, string userId);
    }
}
