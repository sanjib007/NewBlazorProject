using L3T.Infrastructure.Helpers.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class CrStatus : AuditableEntity
    {
        public string StatusDisplayName { get; set; }
        public string StatusImage { get; set; }
        public string Status { get; set; }
    }
}
