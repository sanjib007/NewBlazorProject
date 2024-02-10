using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation
{
    public class ClientReqResService : IClientReqResService
    {
        private readonly ClientAPIDBContext _dbContext;
        private readonly ILogger<ClientReqResService> _logger;
        public ClientReqResService(ClientAPIDBContext dbContext, ILogger<ClientReqResService> logger)

        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ApiResponse> BadRequest(string exMessage)
        {
            return new ApiResponse()
            {
                Status = "Error",
                StatusCode = 400,
                Message = exMessage
            };
        }

        public async Task<ApiResponse> CreateResponseRequest(object request, object response, string cusIp, string methodName, string userId, string successMess, string errorMessage = null)
        {
            var newResponse = new ApiResponse();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                newResponse = new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = errorMessage
                };
            }
            else
            {
                newResponse = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = successMess,
                    Data = response
                };
            }

            var reqResModel = new ClientRequestResponseModel()
            {
                Request = JsonConvert.SerializeObject(request),
                Response = Convert.ToString(JsonConvert.SerializeObject(newResponse)),
                MethodName = methodName,
                RequestedIP = cusIp,
                CreatedAt = DateTime.Now,
                UserId = userId,
                ErrorLog = errorMessage
            };
            await _dbContext.RequestResponseLog.AddAsync(reqResModel);
            return newResponse;

        }



        //public async Task CreateResponseRequest(object request, object response, string cusIp, string methordName, string userId)
        //{

        //    var reqResModel = new ClientRequestResponseModel()
        //    {
        //        Request = JsonConvert.SerializeObject(request),
        //        Response = Convert.ToString(JsonConvert.SerializeObject(response)),
        //        MethodName = methordName,
        //        RequestedIP = cusIp,
        //        CreatedAt = DateTime.Now,
        //        UserId = userId,
        //        ErrorLog = "",

        //    };


        //    try
        //    {
        //        var result = await _dbContext.RequestResponseLog.AddAsync(reqResModel);
        //        if (await _dbContext.SaveChangesAsync() < 0)
        //        {
        //            _logger.LogInformation("Notifications saved : " + JsonConvert.SerializeObject(reqResModel));
        //            throw new Exception("Notifications Save has some problem.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation("Notifications saved : " + JsonConvert.SerializeObject(ex));
        //    }
        //}
    }
}

