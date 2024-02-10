using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class RSMfonocRouterUpdateRequestModel
    {
           
        public int RouterBrandId { get; set; }
        public int RouterModelId { get; set; }
        public int RouterRebootTimeId { get; set; }
        public int RouterRebootSettingId { get; set; }
        public string SubscriberCode { get; set; }
       
      
    }
}
