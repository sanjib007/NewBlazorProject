using IPV6ConfigSetup.DataAccess.CommonModel;
using IPV6ConfigSetup.DataAccess.Model;

namespace IPV6ConfigSetup.Services.Interface
{
    public interface IIPVSetupService
    {
        Task<ApiResponse> InsertPrimarySubnet(IPV6_PrimarySubnetModel primaryModel);
        Task<ApiResponse> GetAllPrimarySubnet();
        Task<ApiResponse> InsertDivisionSubnet(List<IPV6_DivisionSubnet32Model> divisionModel);
        Task<ApiResponse> GetAllDivisionSubnet(long primarySubnetId = 0);
        Task<ApiResponse> InsertUserTypeSubnet(List<IPV6_UserTypeSubnet36Model> userTypeModel);
        Task<ApiResponse> GetAllUserTypeSubnet(long DivisionSubnetId = 0);
        Task<ApiResponse> InsertCitySubnet(List<IPV6_CitySubnet44Model> cityModel);
        Task<ApiResponse> GetAllCitySubnet(long userTypeSubnetId = 0);
        Task<ApiResponse> InsertBTSSubnet(List<IPV6_BTSSubnet48Model> btsModel);
        Task<ApiResponse> GetAllBTSSubnet(long citySubnetId = 0);
        Task<ApiResponse> InsertParentSubnet(List<IPV6_ParentSubnet56Model> parentModel);
        Task<ApiResponse> GetAllParentSubnet(long btsSubnetId = 0);
        Task<ApiResponse> InsertCustomerSubnet(List<IPV6_CustomerSubnet64Model> customerModel);
        Task<ApiResponse> GetAllCustomerSubnet(long parentSubnetId = 0);
        Task<ApiResponse> InsertIPV6CustomerSubnet(IPV6SetupRequestModel model);
    }
}
