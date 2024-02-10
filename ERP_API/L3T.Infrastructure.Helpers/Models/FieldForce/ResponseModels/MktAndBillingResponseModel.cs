using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class MktAndBillingResponseModel
    {
        [Key]
        public string? DistributorSubscriberID { get; set; }
        public string? RRPSubscriberID { get; set; }
        public string InsService { get; set; }
        public string Cli_code { get; set; }
        public string Cli_Adr_Code { get; set; }

    }
}
