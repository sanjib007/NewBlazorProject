
using Blazored.LocalStorage;
using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;
using Cr.UI.Data.StateManagement;
using Cr.UI.Services.Interface;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Cr.UI.Services.Implementation

{
    public class ChangeRequirementService : IChangeRequirementService
    {
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }
        private readonly IUserService _userService;
        
        private long maxFileSize = 50 * 1000 * 1000;

        public ChangeRequirementService(
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

        public async Task<ApiResponse<List<ViewChangeReq>>> GetViewChangeReqAllAsync(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode.ToString() == "OK")
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<ViewChangeReq>>>(responseBody));
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<List<ViewChangeReq>>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }


        public async Task<ApiResponse> GetChangeReqByIdAsync(string requestUri, int Id)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri + Id);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();
            
            return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse>(responseBody));
        }


        public async Task<ApiResponse<TempChangeRequestedInfo>> StepOneRequest(StepOneRequestModel reqestModel)
        {
            var request = new AddChangeReq();
            request.Subject = reqestModel.Subject;
            request.ChangeRequestFor = reqestModel.ChangeRequestFor;
            request.StepNo = "1";

            return await SaveAsync("TempChangeRequest/AddTempChangeRequest", request, null);
        }

        public async Task<ApiResponse<TempChangeRequestedInfo>> StepTwoRequest(StepTwoRequestModel reqestModel)
        {
            var request = new AddChangeReq();
            request.ChangeFromExisting = reqestModel.ChangeFromExisting;
            request.ChangeToAfter = reqestModel.ChangeToAfter;
            request.ChangeImpactDescription = reqestModel.ChangeImpactDescription;
            request.Justification = reqestModel.Justification;
            request.StepNo = "2";

            return await SaveAsync("TempChangeRequest/AddTempChangeRequest", request, null);
        }

        public async Task<ApiResponse<TempChangeRequestedInfo>> StepThreeRequest(StepThreeRequestModel reqestModel, IBrowserFile files)
        {
            var request = new AddChangeReq();
            request.AddReference = reqestModel.AddReference;
            request.StepNo = "3";
            return await SaveAsync("TempChangeRequest/AddTempChangeRequest", request, files);
        }

        public async Task<ApiResponse<TempChangeRequestedInfo>> StepFourRequest(StepFourRequestModel reqestModel)
        {
            var request = new AddChangeReq();
            request.LevelOfRisk = reqestModel.LevelOfRisk;
            request.LevelOfRiskDescription = reqestModel.LevelOfRiskDescription;
            request.AlternativeDescription = reqestModel.AlternativeDescription;
            request.StepNo = "4";
            return await SaveAsync("TempChangeRequest/AddTempChangeRequest", request, null);
        }


        public async Task<ApiResponse<TempChangeRequestedInfo>> SaveAsync(string requestUri, AddChangeReq obj, IBrowserFile files)
        {
            try
            {
                
                var token = await _localStorageService.GetItemAsync<string>("accessToken");
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using var content = new MultipartFormDataContent();

                if (files != null)
                {
                    //content.Add()
                    //foreach (var item in files)
                    //{
                    //    var fileContent = new StreamContent(item.OpenReadStream(maxFileSize));
                    //    fileContent.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);


                    //    //content.Add(fileContent, "\"TestAttachFile\"");
                    //}
                    var fileContent = new StreamContent(files.OpenReadStream(maxFileSize));
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(files.ContentType);
                    content.Add(fileContent, "\"AttachFile\"", files.Name);
                }
                content.Add(new StringContent(obj.Subject == null ? string.Empty : obj.Subject), "Subject");
                content.Add(new StringContent(obj.ChangeRequestFor == null ? string.Empty : obj.ChangeRequestFor), "ChangeRequestFor");
                content.Add(new StringContent(obj.AddReference == null ? string.Empty : obj.AddReference), "AddReference");
                content.Add(new StringContent(obj.ChangeFromExisting == null ? string.Empty : obj.ChangeFromExisting), "ChangeFromExisting");
                content.Add(new StringContent(obj.ChangeToAfter == null ? string.Empty : obj.ChangeToAfter), "ChangeToAfter");
                content.Add(new StringContent(obj.Justification == null ? string.Empty : obj.Justification), "Justification");
                content.Add(new StringContent(obj.ChangeImpactDescription == null ? string.Empty : obj.ChangeImpactDescription), "ChangeImpactDescription");
                content.Add(new StringContent(obj.LevelOfRisk == null ? string.Empty : obj.LevelOfRisk), "LevelOfRisk");
                content.Add(new StringContent(obj.LevelOfRiskDescription == null ? string.Empty : obj.LevelOfRiskDescription), "LevelOfRiskDescription");
                content.Add(new StringContent(obj.AlternativeDescription == null ? string.Empty : obj.AlternativeDescription), "AlternativeDescription");
                content.Add(new StringContent(obj.StepNo), "StepNo");


                requestMessage.Content = content;

                var response = await _httpClient.SendAsync(requestMessage);

                var responseStatusCode = response.StatusCode.ToString();
                var responseBody = await response.Content.ReadAsStringAsync();
                
                if (responseStatusCode == "OK")
                {
                    var returnedObj = JsonConvert.DeserializeObject<ApiResponse<TempChangeRequestedInfo>>(responseBody);
                    return await Task.FromResult(returnedObj);
                }
                var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
                return new ApiResponse<TempChangeRequestedInfo>()
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

        public async Task<ApiResponse> StepFiveRequest()
        {
            
            var requestUri = "ChangeRequest/AddChangeRequest";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

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

        public async Task<ApiResponse<List<CrAttatchedFile>>> RemovedFile(string requestUri, RemovedFileRequestModel obj)
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
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse<List<CrAttatchedFile>>>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<List<CrAttatchedFile>>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse<List<CrAttatchedFile>>> GetAllFiles(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse<List<CrAttatchedFile>>>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<List<CrAttatchedFile>>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }


        public async Task<ApiResponse> DeleteAsync(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

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

        public async Task<ApiResponse> ChangeTaskAsync(string requestUri)
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

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


        public async Task<ApiResponse<PaginationModel<List<ChangeRequestModel>>>> GetAllCrList(string requestUri, ChangeRequestListRequestModel obj)
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
            
            if (responseStatusCode.ToString() == "OK")
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<PaginationModel<List<ChangeRequestModel>>>>(responseBody));
            }
            var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<PaginationModel<List<ChangeRequestModel>>>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }


        public async Task<ApiResponse> AddAssignEmployeeSaveAsync(string requestUri, AddAssignEmployeeReq obj)
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

        public async Task<ApiResponse> AddRemark(string requestUri, AddRemarkRequestModel obj)
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

        public async Task<ApiResponse> ChangeCrStaurs(string requestUri)
        {
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

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
