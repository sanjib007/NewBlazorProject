using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class ClientBillingAddressModel
    {
        [Key]
        public string br_contact_name { get; set; }
        public string? br_adr1 { get; set; }
        public string? br_adr2 { get; set; }
        public string? br_area { get; set; }
        public string? br_sub_area { get; set; }
        public string? br_contact_num { get; set; }
        public string? br_contact_email { get; set; }
        
    }
}
