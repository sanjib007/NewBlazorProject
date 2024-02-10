using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.OAuth2DotNet7.DataAccess.Model.Parmission.RequestModel
{
    public class GetAllMenuSetupAndPermissionRequestModel
    {
        public string? UserId { get; set; }
        public string roleName { get; set; }
        public string? projectName { get; set; }

    }
}
