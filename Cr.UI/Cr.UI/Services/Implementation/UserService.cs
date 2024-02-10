using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Blazored.LocalStorage;
using System.Security.Claims;
using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;
using Cr.UI.Services.Interface;
using Cr.UI.Data.StateManagement;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using Cr.UI.Data.Permission;

namespace Cr.UI.Services.Implementation
{
    public class UserService : IUserService
    {
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<UserService> _logger;

        public UserService(
            HttpClient httpClient, 
            IOptions<AppSettings> appSettings, 
            ILocalStorageService localStorageService,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _appSettings = appSettings.Value;
            httpClient.BaseAddress = new Uri(_appSettings.BaseAddress);
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> Encryption(string Data)
        {
            var methodName = "UserService/Encryption";
            try
            {
                string output = string.Empty;
                byte[] byteData = Encoding.UTF8.GetBytes(Data);
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "key", "mycert.pem");
                    var collection = new X509Certificate2Collection();
                    collection.Import(path);
                    var certificate = collection[0];
                    using (RSA csp = (RSA)certificate.PublicKey.Key)
                    {
                        byte[] bytesEncrypted = csp.Encrypt(byteData, RSAEncryptionPadding.OaepSHA1);
                        output = Convert.ToBase64String(bytesEncrypted);
                    }
                }
                return output.Replace('/', '*');
            }
            catch (CryptographicException ex)
            {
                _logger.LogInformation("Method Name: " + methodName + "Error: " + JsonConvert.SerializeObject(ex) + "Request: " + JsonConvert.SerializeObject(Data));

                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<UserModel> LoginAsync(UserModel user)
        {
            var methodName = "UserService/LoginAsync";
            
            try
            {
                string secret = _appSettings.ClientSecret;

                var formContent = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string, string>("username", user.UserName),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "Test"),
                    new KeyValuePair<string, string>("client_secret", secret),
                    new KeyValuePair<string, string>("scope", "offline_access")
                });

                string serializedUser = JsonConvert.SerializeObject(formContent);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "connect/token");
                requestMessage.Content = formContent; //new StringContent(serializedUser);

                requestMessage.Content.Headers.ContentType
                    = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await _httpClient.SendAsync(requestMessage);

                var responseStatusCode = response.StatusCode.ToString();
                var responseBody = await response.Content.ReadAsStringAsync();
                UserModel returnedUser = new UserModel();
                
                if (responseStatusCode == "OK")
                {
                    if(responseBody.Contains("\"status\":\"Error\"") && responseBody.Contains("\"statusCode\":\"400\""))
                    {
                        var responseSet = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                        returnedUser.Status = responseSet.Status;
                        returnedUser.Message = responseSet.Message;
                    }
                    else
                    {
                        returnedUser = JsonConvert.DeserializeObject<UserModel>(responseBody);
                    }                    
                }

                return await Task.FromResult(returnedUser);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Method Name: " + methodName + "Error: " + JsonConvert.SerializeObject(ex) + "Request: " + JsonConvert.SerializeObject(user));
                
                return new UserModel();
            }

        }

        public async Task<ApiResponse> RegisterUserAsync(RegistrationModel user)
        {
            
            //user.Password = Utility.Encrypt(user.Password);
            string serializedUser = JsonConvert.SerializeObject(user);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Authorization/Registration");
            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (responseStatusCode == "OK")
            {
                var returnedUser = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return await Task.FromResult(returnedUser);
            }
            else
            {
                var returnedUser = JsonConvert.DeserializeObject<ValidationErrorModel>(responseBody);
                var error = new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "wrong",
                    Data = returnedUser
                };
                return await Task.FromResult(error);
            }
        }

        public async Task<ApiResponse> RefreshTokenAsync()
        {
            
            var refreshToken = await _localStorageService.GetItemAsync<string>("refreshToken");
            var formContent = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", "Test"),
                new KeyValuePair<string, string>("client_secret", "test123"),
                new KeyValuePair<string, string>("scope", "offline_access")
            });

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "connect/token");
            requestMessage.Content = formContent;

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            var returnedUser = new UserModel();
            
            if (responseStatusCode == "OK")
            {
                returnedUser = JsonConvert.DeserializeObject<UserModel>(responseBody);
                return new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = await Task.FromResult(returnedUser)
                };
            }
            return await ErrorMethod(responseStatusCode, responseBody);
        }


        public async Task Logout()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "connect/logout");

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);
            await response.Content.ReadAsStringAsync();
        }

        public async Task<ApiResponse<PaginationModel<List<UserListModel>>>> GetAllUserList(GetUserFilterModel model)
        {
            
            string serializedUser = JsonConvert.SerializeObject(model);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Account/GetAllUsers");
            requestMessage.Content = new StringContent(serializedUser);

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseStatusCode = response.StatusCode.ToString();
            var returnedUser = new ApiResponse<PaginationModel<List<UserListModel>>>();
            
            if (responseStatusCode == "OK")
            {
                returnedUser = JsonConvert.DeserializeObject<ApiResponse<PaginationModel<List<UserListModel>>>>(responseBody);
                return returnedUser;
            }
            var errorRes = await ErrorMethod(responseStatusCode, responseBody);
            returnedUser.Status = errorRes.Status;
            returnedUser.StatusCode = errorRes.StatusCode;
            returnedUser.Message = errorRes.Message;
            return returnedUser;
        }

        public async Task setLocalStorage(UserModel user)
        {
            await _localStorageService.SetItemAsync("accessToken", user.AccessToken);
            await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);
            var nowDate = DateTime.Now;
            await _localStorageService.SetItemAsync("expire_in", nowDate.AddSeconds(user.expires_in));
            await _localStorageService.SetItemAsync("fullName", user.FullName);
            await _localStorageService.SetItemAsync("subject", user.Subject);
            await _localStorageService.SetItemAsync("email", user.Email);
            await _localStorageService.SetItemAsync("designation", user.User_designation);
            await _localStorageService.SetItemAsync("department", user.Department);
            await _localStorageService.SetItemAsync("roles", user.Roles);
            await _localStorageService.SetItemAsync("imgUrl", _appSettings.ImageUrlAddress);
        }

        public async Task removedLocalStorage()
        {
            List<string> removeList = new List<string>()
            {
                "refreshToken",
                "accessToken",
                "expire_in",
                "fullName",
                "subject",
                "email",
                "designation",
                "department",
                "InactiveClass",
                "ActiveClass",
                "CompleteClass",
                "StepImage",
                "uncompleteCr",
                "CrSteps",
                "roles",
                "imgUrl",
                "permissionCount",
                "permissionStrList",
                "menuListOnly",
            };
            var permissionCount = await _localStorageService.GetItemAsync<int>("permissionCount");
            for (var i = 0; i <= (permissionCount / 30); i++)
            {
                if (i == 0)
                {
                    removeList.Add("permission");
                }
                else
                {
                    removeList.Add($"permission{i}");
                }
            }
            await _localStorageService.RemoveItemsAsync(removeList);
            ClaimsIdentity identity = new ClaimsIdentity();
        }

        public async Task<ApiResponse> ErrorMethod(string statusCode, string responseBody)
        {
            
            ApiResponse response = new ApiResponse();
            if (statusCode == "Unauthorized")
            {
                response.Status = "Error";
                response.StatusCode = 401;
                response.Message = "Refresh token is expire or unauthorized access";
            }
            else if (statusCode == "BadRequest")
            {
                if (responseBody.Contains("Error") && responseBody.Contains("400"))
                {
                    var data = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                    response = data;
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<BadRequestErrorModel>(responseBody);
                    response = new ApiResponse()
                    {
                        Status = "invalid_grant",
                        StatusCode = 400,
                        Message = data.error_description
                    };
                }


            }
            else
            {
                if (!string.IsNullOrWhiteSpace(responseBody))
                    response = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                else
                {
                    response.Status = statusCode;
                    response.StatusCode = 404;
                    response.Message = "Not Found.";
                }
            }
            
            return response;
        }


        public async Task<List<CrCreateStepsModel>> ChangeSteps(List<CrCreateStepsModel> steps, int currentStep)
        {
            
            var presentStep = steps.FirstOrDefault(x => x.StapNumber == currentStep);
            presentStep.StapImage = await _localStorageService.GetItemAsync<string>("StepImage");
            presentStep.StapClass = await _localStorageService.GetItemAsync<string>("CompleteClass");
            presentStep.IsCompleted = true;
            var secondStep = steps.FirstOrDefault(x => x.StapNumber == currentStep + 1);
            secondStep.StapClass = await _localStorageService.GetItemAsync<string>("ActiveClass");
            secondStep.IsCompleted = true;
            await _localStorageService.SetItemAsync("CrSteps", steps);
            
            return steps;
        }
    }
}
