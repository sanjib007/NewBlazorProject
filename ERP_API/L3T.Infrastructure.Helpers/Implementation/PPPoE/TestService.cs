using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.DataContext.PPPoEDBContext;
using L3T.Infrastructure.Helpers.Interface.PPPoE;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tik4net;

namespace L3T.Infrastructure.Helpers.Implementation.PPPoE
{
    public class TestService:ITestService
    {
        private readonly ILogger<TestService> _logger;
        private readonly PPPoEDBContext _pppoEDBContext;
        public TestService(ILogger<TestService> logger, PPPoEDBContext pppoEDBContext) {
            _logger = logger;
            _pppoEDBContext = pppoEDBContext;
        }
        public async Task<ApiResponse> GetRadcheck(int id)
        {
            var methordName = "TestService/GetUserInfoFromMikrotikRouter";
            try
            {
                var apiResponse = new ApiResponse()
                {
                    Message = "get data",
                    Status = "success",
                    StatusCode = 200,
                    Data = id
                };
                await InsertRequestResponse(id, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                await InsertRequestResponse(id, response, null, null, methordName, null);
                return response;
            }
        }


        public async Task<ApiResponse> GetTestList(int id)
        {
            var methordName = "CamsService/GetUserInfoFromMikrotikRouter";
            try
            {
                var apiResponse = new ApiResponse()
                {
                    Message = "get data",
                    Status = "success",
                    StatusCode = 200,
                    Data = id
                };
                await InsertRequestResponse(id, apiResponse, null, null, methordName, null);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methordName);
                var response = new ApiResponse()
                {
                    Message = ex.Message,
                    Status = "Error",
                    StatusCode = 400
                };
                await InsertRequestResponse(id, response, null, null, methordName, null);
                return response;
            }
        }


        private async Task InsertRequestResponse(object request, object response, string cusIp, string routerIp, string methordName, string userId, string subId = null)
        {
            var errorMethordName = "CamsService/InsertRequestResponseSync";
            try
            {
                var reqResModel = new PPPoERequestResponseModel()
                {
                    Request = JsonConvert.SerializeObject(request),
                    Response = System.Convert.ToString(JsonConvert.SerializeObject(response)),
                    MethordName = methordName,
                    CustomerIp = cusIp,
                    RouterIp = routerIp,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    SubId = subId
                };
                _pppoEDBContext.PPPoERequestResponse.Add(reqResModel);
                _pppoEDBContext.SaveChanges();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
            }
            catch (Exception ex)
            {
                string errormessage = errorMethord(ex, errorMethordName).Result;
            }
        }

        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }
    }
}
