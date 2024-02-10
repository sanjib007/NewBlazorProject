using IPV6ConfigBlazorWA.Model.DataModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace IPV6ConfigBlazorWA.Model
{
    public class IPV6ConfigSetupService<T> : IIPV6ConfigSetupService<T>
    {
        private readonly HttpClient _httpClient;
        public IPV6ConfigSetupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<T>> SaveAsync(string requestUri, T data)
        {
            try
            {
                string serializedUser = JsonConvert.SerializeObject(data);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

                requestMessage.Content = new StringContent(serializedUser);

                requestMessage.Content.Headers.ContentType
                    = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.SendAsync(requestMessage);

                var responseStatusCode = response.StatusCode.ToString();
                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseStatusCode.ToString() == "OK")
                {
                    var returnedObj = JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody);
                    return returnedObj;
                }
                return new ApiResponse<T>()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "something wrong"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<T>>> GetAllAsync(string requestUri)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

                var response = await _httpClient.SendAsync(requestMessage);

                var responseStatusCode = response.StatusCode.ToString();
                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseStatusCode.ToString() == "OK")
                {
                    var result = await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<List<T>>>(responseBody));
                    return result;
                }
                return new ApiResponse<List<T>>()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = "something wrong"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        

    }
}
