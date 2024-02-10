using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data
{
    public class AppUserModel
    {
        //[Key]
        public long Id { get; set; }
        public string fullName { get; set; }
        public string user_designation { get; set; }
        public string department { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string RoleName { get; set; }
    }
}
