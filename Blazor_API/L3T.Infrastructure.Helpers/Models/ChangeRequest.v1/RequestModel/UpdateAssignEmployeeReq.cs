using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class UpdateAssignEmployeeReq
    {
        public int Id { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public int CrId { get; set; }
        public string Status { get; set; }
        public bool DeleteStatus { get; set; } = false;
        public string? LastModifiedBy { get; set; }

        public string? LastModifiedAt { get; set; }

    }
}
