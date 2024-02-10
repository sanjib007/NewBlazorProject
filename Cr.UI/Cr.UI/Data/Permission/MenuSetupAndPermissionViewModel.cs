namespace Cr.UI.Data.Permission
{
    public class MenuSetupAndPermissionViewModel
    {
        public long Id { get; set; }
        public string? MenuName { get; set; }
        public string? MenuIcon { get; set; }
        public string? MenuPath { get; set; }
        public string? FeatureName { get; set; }
        public string? ControllerName { get; set; }
        public string? MethodName { get; set; }
        public string? ApplicationName { get; set; }
        public string? ApplicationBaseUrl { get; set; }
        public int ParentId { get; set; }
        public int MenuSequence { get; set; }
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public bool ShowInMenuItem { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? AllowAnonymous { get; set; }
        public long? MenuSetupId { get; set; }
        public long? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? UserId { get; set; }
    }
}
