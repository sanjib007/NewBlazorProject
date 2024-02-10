using Newtonsoft.Json;

namespace L3T.Infrastructure.Helpers.Models.ThirdPartyModel
{
	public class UserModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Source { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string PhoneNo { get; set; }
		public string Subject { get; set; }
		public string Roles { get; set; }
		public string User_designation { get; set; }
		public string Department { get; set; }

		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		public string grant_type { get; set; }
		public string client_id { get; set; }
		public string client_secret { get; set; }
		public string scope { get; set; }
		public int expires_in { get; set; }

		public string Status { get; set; }
		public string Message { get; set; }
	}
}
