using Microsoft.AspNetCore.Identity;

namespace L3TIdentityOAuth2Server.DataAccess.IdentityModels
{
    public class AppUser : IdentityUser<long>, ITrackable
    {
        public string FullName { get; set; }
        public string? Userid { get; set; }
        public string? User_name { get; set; }
        public string? User_designation { get; set; }
        public string? Department { get; set; }
        public string? User_email { get; set; }
        public string? Status { get; set; }
        public string? Resign_date { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Father_Name { get; set; }
        public string? Mother_Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Post_Code { get; set; }
        public string? Country { get; set; }
        public string? Permanent_Address { get; set; }
        public string? Permanent_City { get; set; }
        public string? Permanent_state { get; set; }
        public string? Permanent_Post { get; set; }
        public string? Permanent_Country { get; set; }
        public string? Religion { get; set; }
        public string? DOB { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Children { get; set; }
        public string? Gender { get; set; }
        public string? Blood_Group { get; set; }
        public string? Homephone { get; set; }
        public string? Workphone { get; set; }
        public string? HandSet { get; set; }
        public string? Join_Date { get; set; }
        public string? Confrim_Date { get; set; }
        public string? TIN { get; set; }
        public string? Section { get; set; }
        public string? Office { get; set; }
        public string? WorkLocation { get; set; }


        public bool IsLoginWithAD { get; set; } = false;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeviceId { get; set; }
    }
}