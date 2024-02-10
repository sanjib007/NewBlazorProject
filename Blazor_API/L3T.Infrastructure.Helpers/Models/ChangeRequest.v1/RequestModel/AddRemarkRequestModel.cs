using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class AddRemarkRequestModel
    {
        public long CrId { get; set; }
        public string? Remark { get; set; }
    }
}
