namespace L3T.OAuth2DotNet7.DataAccess.Model
{
    public class IdentityRequestResponseModel
    {
        public long Id { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? UserId { get; set; }
        public string? ErrorLog { get; set; }
        public string? RequestedIP { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? MethodName { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
