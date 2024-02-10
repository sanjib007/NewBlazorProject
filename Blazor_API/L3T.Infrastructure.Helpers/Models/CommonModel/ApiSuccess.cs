namespace L3T.Infrastructure.Helpers.Models.CommonModel
{
    public class ApiSuccess
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}