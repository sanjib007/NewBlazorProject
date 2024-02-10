using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.PPPoE
{
    public interface ITestService
    {
        // Task is for Async method
        // ApiResponse is for sync method 
        Task<ApiResponse> GetTestList(int id);
      

    }
}
