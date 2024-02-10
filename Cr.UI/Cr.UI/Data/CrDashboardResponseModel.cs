using Cr.UI.Data.ChangeRequirementModel;

namespace Cr.UI.Data
{
    public class CrDashboardResponseModel
    {
        public List<GetAllTotalCrByCatagoryWise> getAllTotalCrByCatagoryWises { get; set; }
        public List<ChangeRequestModel> LastFiveCrInfo { get; set; }
    }
}
