using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class RSMInstallationFileUploadRequestModel
    {
        public string RefNo { get; set; }
        public string SubscriberId { get; set; }
        public string FileType { get; set; }
        public IFormFile FileDetails { get; set; }

    }
}
