using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class InitialServiceRestoredNotificationModel
    {
        [Key]
        public string comp_info_ref_no { get; set; }
        public string? comp_info_con_email { get; set; }
        public string? brCliCode { get; set; }
        public int? brSlNo { get; set; }
        public string? phone_no { get; set; }
        public string? MailData { get; set; }
    }
}
