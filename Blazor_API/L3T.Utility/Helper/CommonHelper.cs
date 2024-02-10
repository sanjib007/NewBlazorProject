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
        };

        public const string Version = "1.0";
        public const string ControllerRoute = "api/[controller]";

        // this is for role set
        public const string AllRole = "User, Admin, SuperAdmin, Approver, SuperViewer, Developer";
        public const string AllAdminRole = "Admin, SuperAdmin";
        public const string UserRole = "User";
        public const string AdminRole = "Admin";
        public const string SuperAdminRole = "SuperAdmin";
        public const string FileExtentionList = "jpg,jpeg,png,pdf,doc,docx,xls,xlsx";

    }


}
