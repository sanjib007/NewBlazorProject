using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AssignEmployeeListReqModel
    {
        public string? CrId { get; set; }
        public string? EmpId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;

    }
}
