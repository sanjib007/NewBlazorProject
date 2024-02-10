namespace L3T.Infrastructure.Helpers.CommonModel
{
    public static class ApiError
    {
       
        public static ApiData OutputErrorResponse(string message, int statusCode,
          string details)
        {
            return new ApiData()
            {
                statusCode = statusCode,
                Status = "Fail",
                message = message,
                details = details
            };
        }
    }

    public class ApiData
    {
        public int statusCode { get; set; }
        public string Status { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    }

  
}