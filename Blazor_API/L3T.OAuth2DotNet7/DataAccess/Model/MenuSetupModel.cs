namespace L3T.OAuth2DotNet7.DataAccess.Model
{
    public class MenuSetupModel
    {
        public long Id { get; set; }
        public string? MenuName { get; set; }
        public string? FeatureName { get; set; }
        public string? ControllerName { get; set; }
        public string? MethodName { get; set; }
        public string? ApplicationName { get; set; }
        public string? ApplicationBaseUrl { get; set; }
        public long ParentId { get; set; }
        public bool IsActive { get; set; }
        public bool ShowInMenuItem { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
