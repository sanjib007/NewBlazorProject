using L3T.Infrastructure.Helpers.Extention;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel
{
    [Keyless]
    public class SP_AssignEmployeeListResponse 
    {
        public long Id { get; set; }
        public string? EmpId { get; set; }
        public string? EmpName { get; set; }
        public int? CrId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalDay { get; set; }
        public string? Task { get; set; }
        public bool? DeleteStatus { get; set; } = false;
        public int Total { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }

    }
    [Keyless]
    public class ApproximateDate
    {
        public DateTime? EndDate { get; set; }
    }
}
