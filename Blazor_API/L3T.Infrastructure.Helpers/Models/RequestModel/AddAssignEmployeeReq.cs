using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.RequestModel
{
    public class AddAssignEmployeeReq
    {
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public int CrId { get; set; }
        public string? Status { get; set; }
        public bool DeleteStatus { get; set; } = false;
        public string? CreatedBy { get; set; }

    }
}
