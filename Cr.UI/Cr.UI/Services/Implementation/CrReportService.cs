using Blazored.LocalStorage;
using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;
using Cr.UI.Data.StateManagement;
using Cr.UI.Services.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Cr.UI.Services.Implementation
{
    public class CrReportService : ICrReportService
    {
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }
        private readonly IUserService _userService;

        public CrReportService(
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


        public async Task<ApiResponse<PaginationModel<List<AssignEmployeeListResponse>>>> GetAssignEmployeeList(string requestUri, AssignEmployeeListReqModel obj)
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
            
            if (responseStatusCode.ToString() == "OK")
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<PaginationModel<List<AssignEmployeeListResponse>>>>(responseBody));
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<PaginationModel<List<AssignEmployeeListResponse>>>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }
    }
}
