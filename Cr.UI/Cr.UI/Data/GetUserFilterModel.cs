namespace Cr.UI.Data
{
    public class GetUserFilterModel
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? L3TID { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirm { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirm { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
