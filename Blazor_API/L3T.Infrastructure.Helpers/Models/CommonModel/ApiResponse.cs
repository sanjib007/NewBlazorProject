namespace L3T.Infrastructure.Helpers.Models.CommonModel
{
    public class ApiResponse
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

	public class ApiResponse<T>
	{
		public string Status { get; set; }
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }
	}
}