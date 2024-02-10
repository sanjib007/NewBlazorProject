using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class HardwareInfoResponseModel
    {

        [Key]
        public decimal? Otc { get; set; }
        public decimal? Mrc { get; set; }
        public string? BillingCycle { get; set; }
        public string? BillingType { get; set; }
        public string? VatProcess { get; set; }
        public string? Comments { get; set; }
    }
}
