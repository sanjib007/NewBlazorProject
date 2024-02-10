namespace L3T.OAuth2DotNet7.DataAccess.RequestModel
{
    public class ClientInsertRequestModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
        public List<string> Permissions { get; set; }
    }
}
