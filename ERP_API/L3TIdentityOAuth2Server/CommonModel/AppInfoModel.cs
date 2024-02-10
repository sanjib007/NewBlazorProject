namespace L3TIdentityOAuth2Server.CommonModel
{
    public class AppInfoModel
    {
        public long Id { get; set; }
        public string apkVersion { get; set; }
        public string baseOS { get; set; }
        public string osVersion { get; set; }
        public string brand { get; set; }
        public string device { get; set; }
        public string model { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
