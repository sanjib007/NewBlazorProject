using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class CableNetworkResponseModel
    {
        public List<CableNetworkTypeResponseModel> CableNetworkTypeList { get; set; }
        public List<NttnResponseModel> NttnNameList { get; set; }
        public List<TypeOfLinkResponseModel> TypeOfLinkList { get; set;}
    }
}
