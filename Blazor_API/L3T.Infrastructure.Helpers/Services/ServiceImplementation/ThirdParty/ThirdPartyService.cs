using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.ThirdPartyModel;
using L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.ThirdParty;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.ThirdParty
{
	public class ThirdPartyService : IThirdPartyService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICRRequestResponseService _cRRequestResponseService;
		private readonly ILogger<ThirdPartyService> _logger;
		private string _token;
		public ThirdPartyService(
			IHttpClientFactory httpClientFactory,
			ILogger<ThirdPartyService> logger,
			ICRRequestResponseService cRRequestResponseService)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
			_cRRequestResponseService = cRRequestResponseService;
		}

		public async Task<ApiResponse<List<AppUserModel>>> GetAllDefaultApprovalFlow(string l3Id)
		{
			var methodName = "ThirdPartyService/GetAllDefaultApprovalFlow";
			try
			{
				if (string.IsNullOrEmpty(_token))
				{
					await ClientCradentialLogin();
				}
				var requestMessage = new HttpRequestMessage(HttpMethod.Get, $@"Client/ClientGetAllUsersForSearch?searchText={l3Id}");

				requestMessage.Headers.Authorization
					= new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

				var httpClient = _httpClientFactory.CreateClient("apiGateway");
				var response = await httpClient.SendAsync(requestMessage);

				var responseStatusCode = response.StatusCode.ToString();
				var responseBody = await response.Content.ReadAsStringAsync();

				if (responseStatusCode.ToString() == "OK")
				{
					var result = await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<AppUserModel>>>(responseBody));
					await _cRRequestResponseService.CreateResponseRequest(l3Id, result, null, methodName, null, null);
					return result;
				}

				throw new Exception("Something is wrong in third party api request.");
			}
			catch (Exception ex)
			{
				_logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
				await _cRRequestResponseService.CreateResponseRequest(l3Id, ex, null, methodName, null, "Error", ex.Message.ToString());
				return new ApiResponse<List<AppUserModel>>()
				{
					Status = "Error",
					StatusCode = 400,
					Message = ex.Message,
				};

			}
		}

		private async Task<UserModel> ClientCradentialLogin()
		{
			var methodName = "ThirdPartyService/ClientCradentialLogin";
			try
			{
				var formContent = new FormUrlEncodedContent(new[]{
					new KeyValuePair<string, string>("grant_type", "client_credentials"),
					new KeyValuePair<string, string>("client_id", "Test"),
					new KeyValuePair<string, string>("client_secret", "test123"),
					new KeyValuePair<string, string>("scope", "offline_access")
				});

				string serializedUser = JsonConvert.SerializeObject(formContent);

				var requestMessage = new HttpRequestMessage(HttpMethod.Post, "connect/token");
				requestMessage.Content = formContent; //new StringContent(serializedUser);

				requestMessage.Content.Headers.ContentType
					= new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
				
				var httpClient = _httpClientFactory.CreateClient("apiGateway");
				var response = await httpClient.SendAsync(requestMessage);

				var responseStatusCode = response.StatusCode.ToString();
				var responseBody = await response.Content.ReadAsStringAsync();

				if (responseStatusCode.ToString() == "OK")
				{
					var result = await Task.FromResult(JsonConvert.DeserializeObject<UserModel>(responseBody));
					_token = result.AccessToken;
					await _cRRequestResponseService.CreateResponseRequest("Client Credential Login", result, null, methodName, null, null);
					return result;
				}

				throw new Exception("Something is wrong in third party api request.");
			}
			catch (Exception ex)
			{
				_logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
				await _cRRequestResponseService.CreateResponseRequest("Client Credential Login", ex, null, methodName, null, "Error", ex.Message.ToString());
				return new UserModel();

			}
		}


	}
}
