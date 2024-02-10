using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AddChangeReq
    {
        public long TempId { get; set; }
        public string Subject { get; set; }
        public string RequestorName { get; set; }
        public string DepartName { get; set; }
        public string EmployeeId { get; set; }
        public string RequestorDesignation { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public DateTime CrDate { get; set; }
        public string ChangeRequestFor { get; set; }
        public string? ChangeFromExisting { get; set; }
        public string? ChangeToAfter { get; set; }
        public string? ChangeImpactDescription { get; set; }
        public string? Justification { get; set; }
        public string? LevelOfRisk { get; set; }
        public string? LevelOfRiskDescription { get; set; }
        public string? AlternativeDescription { get; set; }
        public string? AddReference { get; set; }
        public string? AttachFile { get; set; }
        public string Status { get; set; }
        public string? DevOpsTask { get; set; }
        public DateTime? ExpectedCompletedDate { get; set; }
        public bool DeleteStatus { get; set; } = false;
        public string? FinalApprovedUserId { get; set; }
        public DateTime? FinalApprovedDate { get; set; }
        public string? ApprovedUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? InterdepartmentalApprovedUserId { get; set; }
        public DateTime? InterdepartmentalApprovedDate { get; set; }
       

    }
}
