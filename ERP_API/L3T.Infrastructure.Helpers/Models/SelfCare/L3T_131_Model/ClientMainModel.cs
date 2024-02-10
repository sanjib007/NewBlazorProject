using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.SelfCare.L3T_131_Model
{
    public class ClientMainModel
    {

        [Key]
        public string brCliCode { get; set; }
        public int? brcategoryID { get; set; }
        public string? DistributorID { get; set; }
        public string? phone_no { get; set; }
        public string? email_id { get; set; }
        public int? brSlNo { get; set; }
    }
}
