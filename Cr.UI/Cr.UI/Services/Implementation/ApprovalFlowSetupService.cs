using Blazored.LocalStorage;
using Cr.UI.Data.StateManagement;
using Cr.UI.Data;
using Cr.UI.Services.Interface;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Cr.UI.Data.Permission;
using Cr.UI.Data.ApprovalFlow;
using System.Net.Http.Headers;

namespace Cr.UI.Services.Implementation
{
	public class ApprovalFlowSetupService : IApprovalFlowSetupService
	{
		public HttpClient _httpClient { get; }
		public AppSettings _appSettings { get; }
		public ILocalStorageService _localStorageService { get; }
		private readonly IUserService _userService;
		
		public ApprovalFlowSetupService(
			HttpClient httpClient,
			IOptions<AppSettings> appSettings,
			ILocalStorageService localStorageService,
			IUserService userService
			)
		{
			_httpClient = httpClient;
			_appSettings = appSettings.Value;
			_localStorageService = localStorageService;
			_httpClient.BaseAddress = new Uri(_appSettings.APIAddress);
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
			_userService = userService;
		}


		public async Task<ApiResponse<List<string>>> GetAllStringList(string requestUri)
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
					var result = await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<string>>>(responseBody));
					return result;
				}
				var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
				return new ApiResponse<List<string>>()
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

		public async Task<ApiResponse<List<CrDefaultApprovalFlow>>> GetAllDefaultApprovalFlow(string requestUri)
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
					var result = await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<CrDefaultApprovalFlow>>>(responseBody));
					return result;
				}
				var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
				return new ApiResponse<List<CrDefaultApprovalFlow>>()
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

		public async Task<ApiResponse> InsertDefaultApprovalFlow(string requestUri, AddCrDefaultApprovalFlowReq obj)
		{
			
			string serializedUser = JsonConvert.SerializeObject(obj);
			var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

			var token = await _localStorageService.GetItemAsync<string>("accessToken");
			requestMessage.Headers.Authorization
				= new AuthenticationHeaderValue("Bearer", token);

			requestMessage.Content = new StringContent(serializedUser);

			requestMessage.Content.Headers.ContentType
				= new MediaTypeHeaderValue("application/json");

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

        public async Task<ApiResponse> DefaultApproverActiveInActive(string requestUri, CrDefaultApprovalFlow obj)
        {

            string serializedUser = JsonConvert.SerializeObject(obj);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new MediaTypeHeaderValue("application/json");

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

        public async Task<ApiResponse> CRAddEmployee(string requestUri, AddUser obj)
        {
            try
            {
                string serializedUser = JsonConvert.SerializeObject(obj);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

                var token = await _localStorageService.GetItemAsync<string>("accessToken");
                requestMessage.Headers.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);

                requestMessage.Content = new StringContent(serializedUser);

                requestMessage.Content.Headers.ContentType
                    = new MediaTypeHeaderValue("application/json");

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<AppUserModel>>> GetEmployeeList(string requestUri)        
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
                    var result = await Task.FromResult(JsonConvert.DeserializeObject<List<AppUserModel>>(responseBody));

                    return new ApiResponse<List<AppUserModel>>()
					{
						Status = "Success",
						StatusCode = 200,
						Data = result.ToList()
					};
                }

                var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
                return new ApiResponse<List<AppUserModel>>()
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
    }
}
