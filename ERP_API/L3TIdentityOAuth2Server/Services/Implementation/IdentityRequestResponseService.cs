using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess;
using L3TIdentityOAuth2Server.DataAccess.NotificationRelatedModels;
using L3TIdentityOAuth2Server.Services.Interface;
using Newtonsoft.Json;

namespace L3TIdentityOAuth2Server.Services.Implementation
{


    public class IdentityRequestResponseService : IIdentityRequestResponseService
    {
        private readonly IdentityTokenServerDBContext _identityContext;
        private readonly ILogger<IdentityRequestResponseService> _logger;

        public IdentityRequestResponseService(IdentityTokenServerDBContext identityContext,
            ILogger<IdentityRequestResponseService> logger)
        {
            _identityContext = identityContext;
            _logger = logger;
        }

        public async Task<ApiSuccess> AddIdentityRequestResponseforPushNotificationAsync(IdentityRequestResponseModel requestModel)
        {
            var errorMethordName = "IdentityRequestResponseService/AddIdentityRequestResponseforPushNotificationAsync";
            try
            {
                var data = await _identityContext.IdentityRequestResponses.AddAsync(requestModel);
                var result = await _identityContext.SaveChangesAsync();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(requestModel));
                var response = new ApiSuccess()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Request-Response Saved in DB successfully."
                };

                return response;
            }
            catch (Exception ex)
            {
                //string errormessage = await errorMethord(ex, errorMethordName);
                //await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                var response = new ApiSuccess()
                {
                    Status = "Success",
                    StatusCode = 400,
                    Message = "Request-Response Save failed in DB successfully."
                };

                return response;
            }


        }
    }
}
