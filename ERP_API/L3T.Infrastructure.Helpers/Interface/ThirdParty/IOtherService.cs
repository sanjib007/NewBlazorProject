using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.ThirdParty
{
    public interface IOtherService
    {
       Task<ApiResponse> GetHydraData(string RefNo, string ip);
        
    }
}
