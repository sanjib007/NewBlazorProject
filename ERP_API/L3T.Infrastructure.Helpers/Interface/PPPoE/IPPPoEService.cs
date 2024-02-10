using L3T.Infrastructure.Helpers.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.PPPoE
{
    public interface IPPPoEService
    {
        Task<ApiResponse> GetRadcheck(string username);
        Task<ApiResponse> CreateUser(string username, string package, string password, string ip);
        Task<ApiResponse> NasCreate(string router_name, string secret, string router_ip);
        Task<ApiResponse> IpPoolCreate(string pool_name, string first_ip, string last_ip);
        Task<ApiResponse> ExpiryUpdate(string client_id, DateTime expiry_date);
        Task<ApiResponse> PackageUpdate(string client_id, string new_package);
        Task<ApiResponse> GetCustomerInfo(string client_id);
    }
}
