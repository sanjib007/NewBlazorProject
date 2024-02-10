using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using L3T.OAuth2DotNet7.DataAccess;
using L3T.OAuth2DotNet7.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace L3T.OAuth2DotNet7.Services.Implementation
{
    public class IdentityReauestResponseService : IIdentityReauestResponseService
    {
        private readonly ILogger<IdentityReauestResponseService> _logger;
        private readonly IdentityTokenServerDBContext _identityDbContext;
        public IdentityReauestResponseService(
            ILogger<IdentityReauestResponseService> logger,
            IdentityTokenServerDBContext identityDbContext)
        {
            _logger = logger;
            _identityDbContext = identityDbContext;
        }


        //public async Task CreateResponseRequest(object request, object response, string cusIp, string methordName, string userId, string errorMessage = null)
        //{
        //    var reqResModel = new IdentityRequestResponseModel()
        //    {
        //        Request = JsonConvert.SerializeObject(request),
        //        Response = Convert.ToString(JsonConvert.SerializeObject(response)),
        //        MethodName = methordName,
        //        RequestedIP = cusIp,
        //        CreatedAt = DateTime.Now,
        //        UserId = userId,
        //        ErrorLog = errorMessage,
        //        CreatedBy = userId

        //    };
        //    await AddRequestResponse(reqResModel);
        //}

        public async Task<ApiResponse> CreateResponseRequest(object request, object response, string cusIp, string methodName, string userId, string successMess = null, string errorMessage = null)
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

            var reqResModel = new IdentityRequestResponseModel()
            {
                Request = JsonConvert.SerializeObject(request),
                Response = Convert.ToString(JsonConvert.SerializeObject(response)),
                MethodName = methodName,
                RequestedIP = cusIp,
                CreatedAt = DateTime.Now,
                UserId = userId,
                ErrorLog = errorMessage,
                CreatedBy = userId

            };
            await AddRequestResponse(reqResModel);

            return newResponse;

        }

        public async Task<ApiResponse> AddRequestResponse(IdentityRequestResponseModel requestResponse)
        {
            var response = new ApiResponse();
            try
            {
                var result = await _identityDbContext.IdentityRequestResponse.AddAsync(requestResponse);

                if (await _identityDbContext.SaveChangesAsync() > 0)
                {
                    response.Status = "Success";
                    response.StatusCode = 200;
                    response.Message = "Notifications saved successfully!";
                    return response;
                }

                _logger.LogInformation("Notifications saved : " + requestResponse);

                throw new Exception("Notifications Save has some problem.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception " + ex.Message.ToString());
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> AddRSAEncryptDataForDuplicationCheck(RSAEncryptDataDuplocationCheckModel request)
        {
            var methodName = "IdentityReauestResponseService/IsHaveEncryptionResult";
            var response = new ApiResponse();
            try
            {
                var result = await IsHaveEncryptionResult(request);
                if (!result)
                {
                    throw new Exception("Invalid encrypt data");
                }

                await _identityDbContext.RSAEncryptDataDuplocationCheck.AddAsync(request);

                if (await _identityDbContext.SaveChangesAsync() > 0)
                {
                    response.Status = "Success";
                    response.StatusCode = 200;
                    response.Message = "Ok";
                    return response;
                }

                _logger.LogInformation(methodName + " : " + request);

                throw new Exception("Some problem.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(methodName + " Exception " + ex.Message.ToString());
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> IsHaveEncryptionResult(RSAEncryptDataDuplocationCheckModel request)
        {
            var methodName = "IdentityReauestResponseService/IsHaveEncryptionResult";
            try
            {
                var result = await _identityDbContext.RSAEncryptDataDuplocationCheck.FirstOrDefaultAsync(x=> x.UserName == request.UserName || x.Password == request.Password);
                
                _logger.LogInformation(methodName +" Request : " + request);
                _logger.LogInformation(methodName + " Result : " + result);
                if (result == null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(methodName + " Exception " + ex.Message.ToString());
                throw new Exception(ex.Message);
            }

        }




    }
}
