using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Extention
{
    public interface ICommonAuditableBaseEntity
    {
       
        string CreatedBy { get; set; }

        DateTime CreatedAt { get; set; }

        string? LastModifiedBy { get; set; }

        DateTime? LastModifiedAt { get; set; }
    }
}
