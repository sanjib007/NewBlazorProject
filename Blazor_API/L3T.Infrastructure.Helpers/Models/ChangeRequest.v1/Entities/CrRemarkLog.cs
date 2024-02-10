using L3T.Infrastructure.Helpers.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class CrRemarkLog : AuditableEntity
    {
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public int CrId { get; set; }       
        public string Message { get; set; }
        public bool DeleteStatus { get; set; } = false;
       
    }
}
