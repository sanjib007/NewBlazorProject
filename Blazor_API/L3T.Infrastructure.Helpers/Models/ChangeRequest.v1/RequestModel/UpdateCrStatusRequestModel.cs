using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class UpdateCrStatusRequestModel
    {
        public long CrId { get; set; }
        public string CrStatus { get; set; }
        public string Email { get; set; }
    }
}
