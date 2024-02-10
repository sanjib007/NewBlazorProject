using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class HydraPackageInformationModel
    {
        [Key]
        public string? CUSTOMER_ID { get; set; }
        public string? SERVICE_NAME { get; set; }
        public string? SERVICE_AMOUNT { get; set; }
        public string? MRC { get; set; }
        public string? SERVICE_END_DATE { get; set; }
    }
}
