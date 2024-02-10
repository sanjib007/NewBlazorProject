using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Utility.Helper
{
    public static class CommonHelper
    {
        public enum Permissions
        {
            Token,
            ClientCredentials,
            Password,
            RefreshToken,
            Email,
            Profile,
            Roles,
            Introspection
        }
    }
}
