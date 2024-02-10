using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class InternetInfoResponseModel
    {
        [Key]
        public decimal? Otc { get; set; }
        public decimal? Mrc { get; set; }
        public string? BillingCycle { get; set; }
        public string? BillingType { get; set; }
        public string? VatProcess { get; set; }
        public string? Comments { get; set; }
        public string InternetBandwidthCIR { get; set; }
        public string InternetBandwidthMIR { get; set; }
        public string VsatBandwidthDownCir { get; set; }
        public string VsatBandwidthDownMir { get; set; }
        public string NoteBandwith { get; set; }
        public string BtsSetupName { get; set; }
        public string MediaName { get; set; }
    }
}
