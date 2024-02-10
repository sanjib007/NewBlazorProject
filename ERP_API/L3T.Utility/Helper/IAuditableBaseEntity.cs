using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Utility.Helper
{
    internal interface IAuditableBaseEntity
    {
        long Id { get; set; }
        string CreatedBy { get; set; }

        DateTime CreatedAt { get; set; }

        string? LastModifiedBy { get; set; }

        DateTime? LastModifiedAt { get; set; }
    }
}
