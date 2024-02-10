using Newtonsoft.Json;

namespace L3TIdentityOAuth2Server.CommonModel
{
    public class PushNotificationResponseModel
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
