using L3T.Infrastructure.Helpers.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities
{
    public class DeveloperInformation : AuditableEntity
    {
        public string UserId { get; set; }
        public string User_Name { get; set; }
        public string User_Email { get; set; }
        public string User_Designation { get; set; }
        public string Department_Code { get; set; }
        public string Department { get; set; }
        public int Status { get; set; }
       
    }
}
