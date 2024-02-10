using L3T.Infrastructure.Helpers.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class CRRequestResponseModel : AuditableEntity
    {
        
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? UserId { get; set; }
        public string? ErrorLog { get; set; }
        public string? MethodName { get; set; }
        public string? RequestedIP { get; set; }
    }
}
