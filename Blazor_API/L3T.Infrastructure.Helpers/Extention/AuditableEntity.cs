using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Extention
{
    public class AuditableEntity
    {
        public long Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }
    }
}
