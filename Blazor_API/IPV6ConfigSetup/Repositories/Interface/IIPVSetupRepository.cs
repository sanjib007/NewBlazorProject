using IPV6ConfigSetup.DataAccess.Model;

namespace IPV6ConfigSetup.Repositories.Interface
{
    public interface IIPVSetupRepository
    {
        Task InsertPrimarySubnet(IPV6_PrimarySubnetModel primaryModel);
        Task<List<IPV6_PrimarySubnetModel>> GetAllPrimarySubnet();
        Task InsertDivisionSubnet(List<IPV6_DivisionSubnet32Model> divisionModel);
        Task<List<IPV6_DivisionSubnet32Model>> GetAllDivisionSubnet(long primarySubnetId);
        Task InsertUserTypeSubnet(List<IPV6_UserTypeSubnet36Model> userTypeModel);
        Task<List<IPV6_UserTypeSubnet36Model>> GetAllUserTypeSubnet(long DivisionSubnetId);
        Task InsertCitySubnet(List<IPV6_CitySubnet44Model> cityModel);
        Task<List<IPV6_CitySubnet44Model>> GetAllCitySubnet(long userTypeSubnetId);
        Task<long> LastCitySubnetIdFormBTSSubnetTable();
        Task InsertBTSSubnet(List<IPV6_BTSSubnet48Model> btsModel);
        Task<List<IPV6_BTSSubnet48Model>> GetAllBTSSubnet(long citySubnetId);
        Task<long> LastBTSSubnetIdFormParentSubnetTable();
        Task<bool> InsertParentSubnet(List<IPV6_ParentSubnet56Model> parentModel);
        Task<List<IPV6_ParentSubnet56Model>> GetAllParentSubnet(long btsSubnetId);
        Task<long> LastPatentSubnetIdFormCustomerSubnetTable();
        Task<bool> InsertCustomerSubnet(List<IPV6_CustomerSubnet64Model> customerModel);
        Task<List<IPV6_CustomerSubnet64Model>> GetAllCustomerSubnet(long parentSubnetId);
        Task<bool> InsertIPV6CustomerSubnet(IPV6SetupRequestModel model);
    }
}
