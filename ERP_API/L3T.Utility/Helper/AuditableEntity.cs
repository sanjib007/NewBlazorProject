using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Utility.Helper
{
    public abstract class AuditableEntity : IAuditableBaseEntity
    {
        public long Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }
    }
}
