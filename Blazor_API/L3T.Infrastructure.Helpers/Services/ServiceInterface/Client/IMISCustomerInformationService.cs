using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Client
{
    public interface IMISCustomerInformationService
    {
        Task<ApiResponse> MisCustomerPhone(string customerId, string getUserid, string ip);
        Task<ApiResponse> MisCustomerCode(string mobileNo, string getUserid, string ip);
    }
}
