namespace L3TIdentityOAuth2Server.DataAccess.NotificationRelatedModels
{
    public class IdentityRequestResponseModel
    {
        public long Id { get; set; }
        public string NotificationType { get; set; }
        public string RequestText { get; set; }
        public string ResponseText { get; set; }
        public string UserId { get; set; }
        public string ErrorLog { get; set; }
        public string RequestedIP { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MethodName { get; set; }
    }
}
