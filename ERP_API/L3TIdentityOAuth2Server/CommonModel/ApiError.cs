namespace L3TIdentityOAuth2Server.CommonModel
{
    public class ApiError
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    } 
}