using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class IpTelephonyResponseModel
    {
  
            [NotMapped]
            public List<IpTelephonyCpeModel>? IpTelephonyCpeList { get; set; }

            public string? PhoneNo { get; set; }
            public string? NotForIP { get; set; }
            public string? Comments { get; set; }
            public string? ISP { get; set; }
            public DateTime? CommencementDate { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public string? CPE { get; set; }

    }
}
