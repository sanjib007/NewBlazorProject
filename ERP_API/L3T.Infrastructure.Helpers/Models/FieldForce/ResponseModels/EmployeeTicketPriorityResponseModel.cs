﻿using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class EmployeeTicketPriorityResponseModel
    {
        public string? RefNo { get; set; }
        public string? Service { get; set; }
        public string? Status { get; set; }
        public string? Emp_id { get; set; }
        public string? PrioritySet { get; set; }

    }
}
