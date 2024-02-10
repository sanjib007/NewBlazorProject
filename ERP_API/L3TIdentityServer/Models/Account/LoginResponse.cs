using L3TIdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace L3TIdentityServer.Models.Account
{
    public class LoginResponse : ResponseModel
    {
        public string DisplayName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public string TokenType { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public bool PhoneNumberNotConfirmed { get; set; }
        public string PhoneNumber { get; set; }
    }
}
