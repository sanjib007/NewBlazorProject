using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel
{
    public class UpdateStatusReq
    {
        public long Id { get; set; }
        public string StatusDisplayName { get; set; }
        public IFormFile? StatusImage { get; set; }
        public string Status { get; set; }

    }
}
