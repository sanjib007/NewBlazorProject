using Blazored.LocalStorage;
using Cr.UI.Data;
using Cr.UI.Data.Socket;
using Cr.UI.Services.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;

namespace Cr.UI.Services.Implementation
{
    public class SocketSerivce : ISocketService
    {
        private readonly IUserService _userService;
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }

        public SocketSerivce(HttpClient httpClient, IOptions<AppSettings> appSettings, ILocalStorageService localStorageService, IUserService userService)
        {
            _userService = userService;
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
            httpClient.BaseAddress = new Uri(_appSettings.SocketUrl);
            _localStorageService = localStorageService;
        }

        


        //public async Task CallChartEndpoint()
        //{
        //    var result = await _client.GetAsync("chart");
        //    if (!result.IsSuccessStatusCode)
        //        Console.WriteLine("Something went wrong with the response");
        //}

        public async Task<ApiResponse> CallChartEndpoint(string requestUri, GetUserInfoFromMikrotikRequestModel model)
        {
            string serializedUser = JsonConvert.SerializeObject(model);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            //var token = await _localStorageService.GetItemAsync<string>("accessToken");
            //requestMessage.Headers.Authorization
            //    = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return await Task.FromResult(returnedObj);
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
