using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class ShowTechnicalInformationFromHydraModel
    {
        public string IP { get; set; }
        public string Gateway { get; set; }
        public string Subnet_Mask { get; set; }
        public string VLAN { get; set; }
        public string OLT { get; set; }
        public string OLT_IP { get; set; }
        public string PON { get; set; }
        public string PORT { get; set; }
        public string Customer_MAC { get; set; }
        public string BRAS { get;set; }
        public string BRAS_IP { get; set; }
    }
}
