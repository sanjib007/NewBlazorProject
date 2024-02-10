using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.RequestModel
{
    public class AddChangeReq
    {
        public string Subject { get; set; }
        public string RequestorName { get; set; }
        public string DepartName { get; set; }
        public string RequestorDesignation { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string CRprojectCheck { get; set; }
        public string CFExistingStatus { get; set; }
        public string CTChangeAfer { get; set; }
        public string Justification { get; set; }
        public string ChangeImpactDescription { get; set; }
        public string RiskFactor { get; set; }

        public string Status { get; set; }
        public string DevOpsTask { get; set; }
        public DateTime ExpectedCompletedDate { get; set; }
        public string Alternatives { get; set; }
        public List<IFormFile>? Attachments { get; set; }
        public string CreatedBy { get; set; }

    }
}
