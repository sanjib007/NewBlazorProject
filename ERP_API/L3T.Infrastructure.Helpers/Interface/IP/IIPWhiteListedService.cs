using L3T.Infrastructure.Helpers.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.IP
{
    public interface IIPWhiteListedService
    {
        Task<ApiResponse> CheckIPWhiteList(string ip, string RequestStr);
    }
}
