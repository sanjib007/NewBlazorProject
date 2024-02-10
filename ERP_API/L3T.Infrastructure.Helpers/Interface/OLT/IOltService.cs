using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.OLT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.OLT
{
    public interface IOltService
    {
        Task<ApiResponse> AddOLTInfo(OltInfo requestModel);
        Task<ApiResponse> GetOltInfo(long Id);
    }
}
