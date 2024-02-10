using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class AllTicketListAndCountRequestModel
    {
        public string EmpId { get; set; }
        public int LastDays { get; set; } = 7;
    }
}
