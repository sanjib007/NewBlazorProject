using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class CrAttatchedFile
    {
        public long Id { get; set; }
        public long CrId { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
