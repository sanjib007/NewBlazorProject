using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Implementation.Client
{
    [Keyless]
    public class TicketListResponseModel
    {
        public string RefNO { get; set; }
        public string? Status { get; set; }
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? InitiateDate { get; set; }
        public string? Area { get; set; }
        public string? brSupportOffice { get; set; }
        public string? phone_no { get; set; }
        public string? email_id { get; set; }
        public string? District { get; set; }
        public string? IsAssignEngineer { get; set; }
        public string? AssignEng { get; set; }
        public string? AssignEmpName { get; set; }
        public DateTime? AssignDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? TicketCategory { get; set; }
        public int Total { get; set; }
    }
}
