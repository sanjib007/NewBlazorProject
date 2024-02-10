using Blazored.LocalStorage;
using Cr.UI.Data.StateManagement;
using Cr.UI.Data;
using Cr.UI.Services.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Cr.UI.Data.Permission;
using System.IO;
using System.Xml.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security;

namespace Cr.UI.Services.Implementation
{
    public class MenuAndPermissionService : IMenuAndPermissionService
    {
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }
        private readonly IUserService _userService;
        

        public MenuAndPermissionService(
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

        public async Task<ApiResponse<PermissionAndSetupModel>> GetAllMenuWithPermission(string requestUri)
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
                    var menuData = await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<PermissionAndSetupModel>>(responseBody));
                    var permission = menuData.Data.AllMenuAndPermissiona.FindAll(x => x.ShowInMenuItem is false);
                    if (permission.Count > 0)
					{
                        await _localStorageService.SetItemAsync($"permissionCount", permission.Count);
                        for ( var i = 0; i <= (permission.Count / 30); i++ )
						{
                            var takeFirst30 = permission.Skip(i * 30).Take(30).ToList();
                            await _localStorageService.SetItemAsync($"permission{i}", JsonConvert.SerializeObject(takeFirst30));
                        }
					}
                    var strList = menuData.Data.SetupedPermission.FindAll(x => x.ShowInMenuItem is false).Select(x => x.ControllerName + "/" + x.MethodName).ToList();
                    string strListToStr = string.Join(",", strList.Select(x => x.ToLower()));
                    await _localStorageService.SetItemAsync("permissionStrList", strListToStr);
                    var menuListOnly = menuData.Data.SetupedPermission.FindAll(x => x.ShowInMenuItem is true).ToList();
                    await _localStorageService.SetItemAsync("menuListOnly", JsonConvert.SerializeObject(menuListOnly));
                    return menuData;
                }
                var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
                return new ApiResponse<PermissionAndSetupModel>()
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

        public async Task<IDictionary<string, bool>> CheckPermission(string name)
        {
            var getPermission = await _localStorageService.GetItemAsync<string>("permission");
            var strToObj = JsonConvert.DeserializeObject<List<MenuSetupAndPermissionViewModel>>(getPermission);

            var newDictionaryListItem = new Dictionary<string, bool>();

            if (strToObj.Count > 0)
            {

				List<string> nameList = name.Split(',').Select(p => p.Trim()).ToList();
				
				foreach (var aName in nameList)
                {

                    var havePermission = strToObj.FirstOrDefault(x => x.MethodName == aName);
                    if (havePermission != null)
                    {
                        newDictionaryListItem.Add(aName, true);
                    }
                    else
                    {
                        newDictionaryListItem.Add(aName, false);
                    }
                }                
                return newDictionaryListItem;
            }

            return null;
        }

		public async Task<ApiResponse<List<MenuSetupModel>>> GetAllMenuAndPermission(string requestUri)
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
					var menuData = await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<MenuSetupModel>>>(responseBody));
					return menuData;
				}
				var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
				return new ApiResponse<List<MenuSetupModel>>()
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

        public async Task<ApiResponse> MenuUpdate(MenuUpdateRequestModel obj)
        {
			MenuSetupRequestModel requestModel = new MenuSetupRequestModel()
            {
                Id = obj.Id,
                MenuName = obj.MenuName,
                MenuIcon = obj.MenuIcon,
                ParentId = obj.ParentId,
                MenuSequence = obj.MenuSequence,
                IsVisible = obj.IsVisible,
                ShowInMenuItem = obj.ShowInMenuItem,
            };
            return await UpdateMenuSetup("MenuSetupAndPermission/SingleMenuUpdate", requestModel);
		}

		public async Task<ApiResponse> PermissionUpdate(PermissionUpdateRequestModel obj)
		{
			MenuSetupRequestModel requestModel = new MenuSetupRequestModel()
			{
				Id = obj.Id,
                FeatureName = obj.FeatureName,
				IsVisible = obj.IsVisible,
                AllowAnonymous = obj.AllowAnonymous,
			};
			return await UpdateMenuSetup("MenuSetupAndPermission/SingleMenuUpdate", requestModel);
		}

		public async Task<ApiResponse> AnonymousPermissionUpdate(AnonymousPermissionRequestModel obj)
		{
			MenuSetupRequestModel requestModel = new MenuSetupRequestModel()
			{
				Id = obj.Id,
				FeatureName = obj.FeatureName,
				IsVisible = obj.IsVisible,
				AllowAnonymous = obj.AllowAnonymous,
			};
			return await UpdateMenuSetup("MenuSetupAndPermission/SingleMenuUpdate", requestModel);
		}

		public async Task<ApiResponse> UpdateMenuSetup(string requestUri, MenuSetupRequestModel obj)
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

		public async Task<ApiResponse<List<string>>> GetAllRolesName(string requestUri)
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
					var responseData = await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<string>>>(responseBody));
					return responseData;
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

		public async Task<List<string>> GetAllUsersBySearch(string requestUri)
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
					var responseData = await Task.FromResult(JsonConvert.DeserializeObject<List<string>>(responseBody));
					return responseData;
				}
				var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiResponse<List<MenuSetupModel>>> SpecificUserOrRoleWisePermission(string requestUri, GetAllMenuSetupAndPermissionRequestModel obj, List<MenuSetupModel> allMenuPermission)
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
				var returnedObj = JsonConvert.DeserializeObject<ApiResponse<List<MenuSetupAndPermissionViewModel>>>(responseBody);
				var AllMenuAndPermission = returnedObj.Data;
				var AllMenuList = AllMenuAndPermission.FindAll(x => x.ShowInMenuItem == true && (x.RoleName == obj.roleName || x.UserId == obj.UserId));
				var AllPermission = AllMenuAndPermission.FindAll(x => x.ShowInMenuItem == false && x.AllowAnonymous == false && (x.RoleName == obj.roleName || x.UserId == obj.UserId));
				var AllAnonymousPermission = AllMenuAndPermission.FindAll(x => x.ShowInMenuItem == false && x.AllowAnonymous == true);

				foreach (var item in allMenuPermission)
				{
					item.IsChecked = false;
					MenuSetupAndPermissionViewModel haveIt = new MenuSetupAndPermissionViewModel();
					MenuSetupAndPermissionViewModel haveIt1 = new MenuSetupAndPermissionViewModel();
					MenuSetupAndPermissionViewModel haveIt2 = new MenuSetupAndPermissionViewModel();

					haveIt = AllMenuList.FirstOrDefault(x => x.Id == item.Id);

					haveIt1 = AllPermission.FirstOrDefault(x => x.Id == item.Id);

					haveIt2 = AllAnonymousPermission.FirstOrDefault(x => x.Id == item.Id);

					if (haveIt != null || haveIt1 != null || haveIt2 != null)
					{
						item.IsChecked = true;
					}
				}

				var responseItem = new ApiResponse<List<MenuSetupModel>>()
				{
					Status = returnedObj.Status,
					StatusCode = returnedObj.StatusCode,
					Message = returnedObj.Message,
					Data = allMenuPermission
				};

				return await Task.FromResult(responseItem);
			}
			var data = await _userService.ErrorMethod(responseStatusCode, responseBody);
			return new ApiResponse<List<MenuSetupModel>>()
			{
				Status = data.Status,
				StatusCode = data.StatusCode,
				Message = data.Message
			};
		}

		public async Task<ApiResponse> UpdateAndDeleteMenuAndPermission(string requestUri, SetPermissionForRoleOrUserRequestModel obj)
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


	}
}
