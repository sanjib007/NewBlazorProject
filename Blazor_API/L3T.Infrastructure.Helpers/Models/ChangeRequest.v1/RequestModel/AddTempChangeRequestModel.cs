using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AddTempChangeRequestModel
    {
        public long? Id { get; set; }
        public string? Subject { get; set; }
        public string? RequestorName { get; set; }
        public string? DepartName { get; set; }
        public string? EmployeeId { get; set; }
        public string? RequestorDesignation { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? ChangeRequestFor { get; set; }
        public string? ChangeFromExisting { get; set; }
        public string? ChangeToAfter { get; set; }
        public string? ChangeImpactDescription { get; set; }
        public string? Justification { get; set; }
        public string? LevelOfRisk { get; set; }
        public string? LevelOfRiskDescription { get; set; }
        public string? AlternativeDescription { get; set; }
        public string? AddReference { get; set; }
        public IFormFile? AttachFile { get; set; }
        //public List<IFormFile>? TestAttachFile { get; set; }
        public string? StepNo { get; set; }
    }
}
