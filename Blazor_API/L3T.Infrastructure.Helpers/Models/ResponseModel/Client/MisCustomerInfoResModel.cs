using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel
{
    [Keyless]
    public class MisCustomerInfoResModel
    {
        public string? brCliCode { get; set; }
        public int? brSlNo { get; set; }
        public string? phone_no { get; set; }
        
    }
}
