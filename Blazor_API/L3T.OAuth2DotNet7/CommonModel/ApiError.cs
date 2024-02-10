namespace L3T.OAuth2DotNet7.CommonModel
{
    public class ApiError
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    }
}