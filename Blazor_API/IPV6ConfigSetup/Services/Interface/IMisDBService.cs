using IPV6ConfigSetup.DataAccess.CommonModel;

namespace IPV6ConfigSetup.Services.Interface
{
    public interface IMisDBService
    {
        Task<ApiResponse> GetAllDistributorList();
        Task<ApiResponse> GetAllBtsList(string customerType, int distributorID, int packageID);
        Task<ApiResponse> GetAllRouterNameList(int btsId);
        Task<ApiResponse> GetRouterInformation(int btsId, string routerID);
        Task<ApiResponse> GetAllPackageNameList(int distributorID);
        Task<ApiResponse> GetBtsWisePoolNameList(string customerType, int btsId, int distributorId, int packageId);
    }
}
