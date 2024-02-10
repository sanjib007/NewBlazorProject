using System.Collections.Generic;

namespace L3TIdentityServer.Models.Account
{
    public class AddUserToRolesRequest
    {
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
