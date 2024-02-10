using System;
using System.Collections.Generic;

namespace L3TIdentityServer.Models.Account
{
    public class MB_GetUserByRoleRequest
    {
        public string RoleName { get; set; }

    }
    public class MB_GetUserByRoleResponse
    {
        private List<UserResponse> _Rows = new List<UserResponse>();
        public List<UserResponse> Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }
    }

    public class UserResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsTemporaryCustomer { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
