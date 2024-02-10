using Blazored.LocalStorage;
using Cr.UI.Data;
using Cr.UI.Data.StateManagement;
using Cr.UI.Services.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;

namespace Cr.UI.Services.Implementation
{
    public class GenericService<T> : IGenericService<T>
    {
        private readonly IUserService _userService;
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }

        public GenericService(
            HttpClient httpClient, 
            IOptions<AppSettings> appSettings, 
            ILocalStorageService localStorageService,
            IUserService userService)
        {
            _appSettings = appSettings.Value;

            httpClient.BaseAddress = new Uri(_appSettings.BaseAddress);

            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _userService = userService;
        }

        public async Task<ApiResponse> DeleteAsync(string requestUri, int Id)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri + Id);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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

        public async Task<ApiResponse<List<T>>> GetAllAsync(string requestUri)
        {

            try
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
                    return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<T>>>(responseBody));
                }
                var data = await _userService.ErrorMethod(responseStatusCode, responseBody);

                return new ApiResponse<List<T>>()
                {
                    Status = data.Status,
                    StatusCode = data.StatusCode,
                    Message = data.Message
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<T>> GetOnlyAsync(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<T>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse> GetWithoutResponse(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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
        

        public async Task<ApiResponse<T>> GetByIdAsync(string requestUri, int Id)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri + Id);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<T>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse<T>> SaveAsync(string requestUri, T obj)
        {
            
            string serializedUser = JsonConvert.SerializeObject(obj);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<T>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse<T>> UpdateAsync(string requestUri, int Id, T obj)
        {
            
            string serializedUser = JsonConvert.SerializeObject(obj);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri + Id);
            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<T>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }
    }
}
