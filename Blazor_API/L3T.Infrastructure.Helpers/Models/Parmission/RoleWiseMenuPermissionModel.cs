using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Parmission
{
    public class RoleWiseMenuPermissionModel
    {
        public long Id { get; set; }
        public long MenuSetupId { get; set; }
        public string RoleName { get; set; }
        public string? UserId { get; set; }
    }
}
