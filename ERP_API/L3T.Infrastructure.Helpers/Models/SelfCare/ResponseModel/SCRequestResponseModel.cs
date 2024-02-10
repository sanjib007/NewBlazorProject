using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SelfCare.ResponseModel
{
    public class SCRequestResponseModel
    {

        public long Id { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? UserId { get; set; }
        public string? ErrorLog { get; set; }
        public string? MethodName { get; set; }
        public string? RequestedIP { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
