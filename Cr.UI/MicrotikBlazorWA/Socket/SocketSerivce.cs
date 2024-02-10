using Newtonsoft.Json;

namespace MicrotikBlazorWA.Socket
{
    public class SocketSerivce : ISocketService
    {
        public HttpClient _httpClient { get; }

        public SocketSerivce(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> CallChartEndpoint(string requestUri, GetUserInfoFromMikrotikRequestModel model)
        {
            string serializedUser = JsonConvert.SerializeObject(model);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);


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

            return new ApiResponse();
        }
    }
}
