namespace Cr.UI.Data
{
    public class DeveloperInformation : AuditableEntity
    {
        public string UserId { get; set; }
        public string User_Name { get; set; }
        public string User_Email { get; set; }
        public string User_Designation { get; set; }
        public string Department_Code { get; set; }
        public string Department { get; set; }
        public int Status { get; set; }
    }
}
