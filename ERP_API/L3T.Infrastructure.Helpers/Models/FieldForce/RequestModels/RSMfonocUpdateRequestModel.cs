using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class RSMfonocUpdateRequestModel
    {
        public string RefNo { get; set; }      
        public string CustomerMac { get; set; }
        public string OnuMac { get; set; }
        public string OnuPort { get; set; }
        public string OnuId { get; set; }
        public string SubscriberCode { get; set; }
        public string ddteamSelectedValue { get; set; }
        public string Remarks { get; set; }
      
    }
}
