namespace L3TIdentityOAuth2Server.DataAccess.RequestModel
{
    public class ClientInsertRequestModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
        public List<string> Permissions { get; set; }
    }
}
