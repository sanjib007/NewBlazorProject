﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel
{
    public class SP_CrReportResponse
    {
        [Key]
        public long Id { get; set; }
        public string? Subject { get; set; }
        public string? RequestorName { get; set; }
        public string? RequestorDesignation { get; set; }
        public DateTime? Date { get; set; }
        public string? EmployeeId { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? CRprojectCheck { get; set; }
        public string? CFExistingStatus { get; set; }
        public string? CTChangeAfer { get; set; }
        public string? Justification { get; set; }
        public string? ChangeImpactDescription { get; set; }
        public string? RiskFactor { get; set; }
        public string? Alternatives { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool? DeleteStatus { get; set; }
        public string? DevOpsTask { get; set; }
        public DateTime? ExpectedCompletedDate { get; set; }
        public string? Status { get; set; }
        public string? EmpName { get; set; }
        public string? Message { get; set; }
        public int Total { get; set; }
        public string? FinalApprovedUserId { get; set; }
        public DateTime? FinalApprovedDate { get; set; }
        public string? ApprovedUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
