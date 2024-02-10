using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientP2PAddressModel
    {
        [Key]
        public string? brName { get; set; }
        public string? contact_det { get; set; }
        public string? phone_no { get; set; }
        public string? email_id { get; set; }
        public string? brAddress1 { get; set; }
        public string? brAddress2 { get; set; }
        public string? brAreaGroup { get; set; }
        public string? brArea { get; set; }
    }
}
