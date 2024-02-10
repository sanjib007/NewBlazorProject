using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.DataContext.IPWhiteListDBContext;
using L3T.Infrastructure.Helpers.Interface.IP;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Implementation.IP
{
    public class IPWhiteListedService : IIPWhiteListedService
    {
        private readonly IpWhiteListedDBContext _ipDBContext;
        private readonly ILogger<IPWhiteListedService> _logger;
        private readonly CamsDataWriteContext _camsDataWriteContext;
        public IPWhiteListedService(IpWhiteListedDBContext ipDBContext, ILogger<IPWhiteListedService> logger, CamsDataWriteContext camsDataWriteContext) {
            _ipDBContext = ipDBContext;
            _logger = logger;
            _camsDataWriteContext = camsDataWriteContext;
        }
        public async Task<ApiResponse> CheckIPWhiteList(string ip, string RequestStr)
        {
            var methodName = "CheckIPWhiteList";
            try
            {
                _logger.LogInformation("Method Name: CheckIPWhiteList, Requested IP is  : " + ip);
                var getIp = await _ipDBContext.WhiteListedIPs.FirstOrDefaultAsync(x => x.Ip == ip);
                if (getIp == null)
                {
                    throw new Exception("Ip is not match. Requested Ip is : " + ip);
                }
                
                var res = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Ip is match."
                };
                _logger.LogInformation("Success Response : " + JsonConvert.SerializeObject(res));
                return res;
            }
            catch (Exception ex)
            {
                var res = new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "Error : " + ex.Message
                };
                _logger.LogInformation("Method Name: CheckIPWhiteList, Error : " + ex.Message);
                await InsertRequestResponse(RequestStr, res, methodName);
                return res;
            }
            
        }

        private async Task InsertRequestResponse(string request, object response, string methordName)
        {
            var reqResModel = new MikrotikRequestResponse()
            {
                Request = request,
                Response = JsonConvert.SerializeObject(response),
                MethordName = methordName,
                CreatedAt = DateTime.Now
            };
            await _camsDataWriteContext.MikrotikRequestResponses.AddAsync(reqResModel);
            await _camsDataWriteContext.SaveChangesAsync();
            _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
        }
    }
}
