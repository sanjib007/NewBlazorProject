using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel
{
     //[Keyless]
    public class NotificationResponseList
    {
        public long Id { get; set; }
        public long CrId { get; set; }
        public String ApproverEmpId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }

        public string? Image { get; set; }
        public string? Type { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }
    }
}
