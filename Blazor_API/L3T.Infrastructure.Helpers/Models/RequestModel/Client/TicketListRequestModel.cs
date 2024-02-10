using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.RequestModel.Client
{
    public class TicketListRequestModel
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? DateTimeSearchTable { get; set; }
        public string? AssignEmpId { get; set; }
        public string? AssignEmpName { get; set; }
        public string? TicketId { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Area { get; set; }
        public string? SupportOffice { get; set; }
        public string? Status { get; set; }
        public string? TicketCatagory { get; set; }
        public string? OrderByTableName { get; set; }
        public string? DescOrAsc { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
