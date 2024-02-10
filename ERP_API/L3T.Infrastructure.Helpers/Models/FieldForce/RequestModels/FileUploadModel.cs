using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class FileUploadModel
    {
        public IFormFile FileDetails { get; set; }
        //public FileType FileType { get; set; }
    }
}
