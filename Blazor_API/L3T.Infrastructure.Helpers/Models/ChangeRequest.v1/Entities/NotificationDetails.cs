using L3T.Infrastructure.Helpers.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class NotificationDetails : AuditableEntity
    {
        public long CrId { get; set; }
        public string ApproverEmpId { get; set; }
        public string? ApproverRole { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }

        public string? Image { get;set;}
        public string? NotifyURL { get; set; }
        public string? Type { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }

    }
}
