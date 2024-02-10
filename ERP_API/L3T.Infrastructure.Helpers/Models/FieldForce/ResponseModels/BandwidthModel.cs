using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class BandwidthModel
    {

        [Key]
        public string InternetBandwidthCIR { get; set; }
        public string InternetBandwidthMIR { get; set; }
        public string VsatBandwidthDownCir { get; set; }
        public string VsatBandwidthDownMir { get; set; }
        public string NoteBandwith { get; set; }
        public string brCliCode { get; set; }
        public int brSlNo { get; set; }
    }
}
