namespace L3T.OAuth2DotNet7.DataAccess.Model.Parmission
{
    public class RoleWiseMenuPermissionModel
    {
        public long Id { get; set; }
        public long MenuSetupId { get; set; }
        public string RoleName { get; set; }
        public string? UserId { get; set; }
    }
}
