using Blazored.LocalStorage;
using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;
using Cr.UI.Data.StateManagement;
using Cr.UI.Services.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;

namespace Cr.UI.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        private readonly ILocalStorageService _localStorageService;
        private readonly IUserService _userService;

        public NotificationService(
            HttpClient httpClient, 
            IOptions<AppSettings> appSettings, 
            ILocalStorageService localStorageService, 
            IUserService userService
            )
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;

            httpClient.BaseAddress = new Uri(_appSettings.BaseAddress);
            _localStorageService = localStorageService;
            _userService = userService;
        }

        public async Task<ApiResponse<PaginationModel<List<NotificationDetailsResponseModel>>>> GetAllNotification(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode.ToString() == "OK")
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<PaginationModel<List<NotificationDetailsResponseModel>>>>(responseBody));
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<PaginationModel<List<NotificationDetailsResponseModel>>>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse> NotificationUnreadToRead(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode.ToString() == "OK")
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse>(responseBody));
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }
    }
}
