using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class HydraPackageAndBalanceResponseModel
    {
        public HydraBalanceInfoModel BalanceInfo { get; set; }
        public List<HydraPackageInformationModel> PackageInfo { get; set; }
    }
}
