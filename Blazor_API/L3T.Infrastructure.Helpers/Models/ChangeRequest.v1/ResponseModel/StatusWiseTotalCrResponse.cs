using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel
{
     //[Keyless]
    public class StatusWiseTotalCrResponse
    {
        public string? Status { get; set; }
        public string? StatusDisplayName { get; set; }
        public int Total { get; set; }
    }
}
