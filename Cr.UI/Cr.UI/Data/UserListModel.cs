namespace Cr.UI.Data
{
    public class UserListModel
    {
        public int id { get; set; }
        //public List<string> roleName { get; set; }
        public string fullName { get; set; }
        public string userid { get; set; }
        public string user_name { get; set; }
        public string user_designation { get; set; }
        public string department { get; set; }
        public string user_email { get; set; }
        public string status { get; set; }
        public string resign_date { get; set; }
        public string first_Name { get; set; }
        public string last_Name { get; set; }
        public string father_Name { get; set; }
        public string mother_Name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string post_Code { get; set; }
        public string country { get; set; }
        public string permanent_Address { get; set; }
        public string permanent_City { get; set; }
        public string permanent_state { get; set; }
        public string permanent_Post { get; set; }
        public string permanent_Country { get; set; }
        public string religion { get; set; }
        public string dob { get; set; }
        public string maritalStatus { get; set; }
        public string children { get; set; }
        public string gender { get; set; }
        public string blood_Group { get; set; }
        public string homephone { get; set; }
        public string workphone { get; set; }
        public string handSet { get; set; }
        public string join_Date { get; set; }
        public string confrim_Date { get; set; }
        public string tin { get; set; }
        public string section { get; set; }
        public string office { get; set; }
        public string workLocation { get; set; }
        public bool isLoginWithAD { get; set; }
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        public string lastUpdatedAt { get; set; }
        public string lastUpdatedBy { get; set; }
        public string userName { get; set; }
        public string normalizedUserName { get; set; }
        public string email { get; set; }
        public string normalizedEmail { get; set; }
        public bool emailConfirmed { get; set; }
        public string passwordHash { get; set; }
        public string securityStamp { get; set; }
        public string concurrencyStamp { get; set; }
        public string phoneNumber { get; set; }
        public bool phoneNumberConfirmed { get; set; }
        public bool twoFactorEnabled { get; set; }
        public string lockoutEnd { get; set; }
        public bool lockoutEnabled { get; set; }
        public int accessFailedCount { get; set; }
    }
}
