using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Implementation.FieldForce;
using L3T.Infrastructure.Helpers.Interface.ThirdParty;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Implementation.ThirdParty
{
    public class OtherService : IOtherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FieldForceService> _logger;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private readonly IConfiguration _configuration;
        public OtherService(IHttpClientFactory httpClientFactory, ILogger<FieldForceService> logger,
            FFWriteDBContext ffWriteDBContext,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _ffWriteDBContext = ffWriteDBContext;
            _configuration = configuration;
        }

        public async Task<ApiResponse> GetHydraData(string RefNo, string ip)
        {
            var methodName = "OtherService/GetHydraData";
            var hydraUrl = _configuration.GetValue<string>("ThirdPartyUrl:hydraySearchapiUrl");
            try
            {
                if (string.IsNullOrEmpty(RefNo))
                {
                    throw new Exception("Provide RefNo");
                }

                var httpRequestMessage = new HttpRequestMessage(
                            HttpMethod.Post,
                            hydraUrl + RefNo);

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<string>(contentStream);
                    var response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Success",
                        Data = result
                    };
                    await InsertRequestResponse(RefNo, response, methodName, ip,null,null);
                    return response;
                }
                throw new Exception("Something is Wrong.");
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(RefNo, ex, methodName, ip, null, ex.Message);
                //await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }


        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }

        private async Task InsertRequestResponse(object request, object response, string methodName, string requestedIP, string userId, string errorLog)
        {
            var errorMethordName = "FieldForceService/InsertRequestResponse";
            try
            {
                var reqResModel = new FFRequestResponseModel()
                {
                    CreatedAt = DateTime.Now,
                    Request = JsonConvert.SerializeObject(request),
                    Response = JsonConvert.SerializeObject(response),
                    RequestedIP = requestedIP,
                    MethodName = methodName,
                    UserId = userId,
                    ErrorLog = errorLog
                };
                await _ffWriteDBContext.fFRequestResponseModels.AddAsync(reqResModel);
                await _ffWriteDBContext.SaveChangesAsync();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
            }
            catch (Exception ex)
            {
                string errormessage = errorMethord(ex, errorMethordName).Result;
            }
        }
    }
}
