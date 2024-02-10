using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AddAssignEmployeeReq
    {
        public long? Id { get; set; }
        public string? EmpId { get; set; }
        public string? EmpName { get; set; }
        public int CrId { get; set; }
        public DateTime StartDate { get; set; }       
        public int TotalDay { get; set; }
        public string? Task { get; set; }       
       
    }
}
