using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ComplainAccessPermissionResponseModel
    {
        [Key]
        public string? MailTo { get; set; }
        public string? MailCC { get; set; }
        public string? MailBody { get; set; }
    }
}
