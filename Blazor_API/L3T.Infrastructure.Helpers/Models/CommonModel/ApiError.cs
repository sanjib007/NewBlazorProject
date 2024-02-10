namespace L3T.Infrastructure.Helpers.Models.CommonModel
{
    public class ApiError
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    }
}