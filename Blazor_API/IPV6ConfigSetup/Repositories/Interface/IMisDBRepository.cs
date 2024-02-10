using IPV6ConfigSetup.DataAccess.Model.MISDBModel;

namespace IPV6ConfigSetup.Repositories.Interface
{
    public interface IMisDBRepository
    {
        Task<List<DistributorViewModel>> GetAllDistributorList();
        Task<List<BTSViewModel>> GetAllBtsList(string customerType, int distributorID, int packageID);
        Task<List<BackboneRouterSwitchViewModel>> GetAllRouterNameList(int btsId);
        Task<BackboneRouterSwitchInformationViewModel> GetRouterInformation(int btsId, string routerID);
        Task<List<PackagePlanViewModel>> GetAllPackageNameList(int distributorID);
        Task<List<PoolNameListModel>> GetBtsWisePoolNameList(string customerType, int btsId, int distributorId, int packageId);
    }
}
