using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel
{
    public class CrDashboardResponseModel
    {
        public List<GetAllTotalCrByCatagoryWise> getAllTotalCrByCatagoryWises { get; set; }
        public List<ChangeRequestedInfo> LastFiveCrInfo { get; set; }
    }
}
