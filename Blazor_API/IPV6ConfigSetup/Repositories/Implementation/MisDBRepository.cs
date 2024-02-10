using IPV6ConfigSetup.DataAccess;
using IPV6ConfigSetup.DataAccess.Model.MISDBModel;
using IPV6ConfigSetup.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IPV6ConfigSetup.Repositories.Implementation
{
    public class MisDBRepository : IMisDBRepository
    {
        private readonly MisDBContext _misDbContext;
        public MisDBRepository(MisDBContext misDbContext)
        {
            _misDbContext = misDbContext;
        }

        public async Task<List<DistributorViewModel>> GetAllDistributorList()
        {
            var query_str = $"select DistributorID,DistributorName from kh_distributorProfile where ServiceAgentType = 'Radius' order by DistributorName";
            var result = await _misDbContext.DistributorList.FromSqlRaw(query_str).ToListAsync();
            return result;
        }

        public async Task<List<BTSViewModel>> GetAllBtsList(string customerType, int distributorID, int packageID)
        {
            var query_str = string.Empty;
            if (customerType == "radius")
            {
                if(distributorID == 0)
                {
                    throw new Exception("Please select distributor");
                }
                if (packageID == 0)
                {
                    throw new Exception("Please select package");
                }
                query_str = $"select BtsSetupID, BtsSetupName from BtsSetup where BtsSetupID in (select distinct BTSID from Kh_IpAddressRadius where DistributorID = {distributorID} and PackageID = {packageID}) order by BtsSetupName";
            }
            else if (customerType == "static")
            {
                query_str = $"select BtsSetupID, BtsSetupName from BtsSetup where BtsSetupID in (select distinct BTSID from Kh_IpAddress where BTSStatus = 1) order by BtsSetupName";
            }
            else
            {
                throw new Exception("Please provide correct data");
            }

            var result = await _misDbContext.BTSList.FromSqlRaw(query_str).ToListAsync();
            return result;
        }

        public async Task<List<BackboneRouterSwitchViewModel>> GetAllRouterNameList(int btsId)
        {
            var query_str = $"select RouterSwitchID,RouterSwitchBrand from tbl_BackboneRouterSwitchsetup where BtsID='{btsId}'";
            var result = await _misDbContext.BackboneRouterSwitchList.FromSqlRaw(query_str).ToListAsync();
            return result;
        }

        public async Task<BackboneRouterSwitchInformationViewModel> GetRouterInformation(int btsId, string routerID)
        {
            var query_str = $"select RouterSwitchID, RouterSwitchModel, HostName, RouterSwitchIP, Noofport from tbl_BackboneRouterSwitchsetup where BtsID='{btsId}' and RouterSwitchID='{routerID}'";
            var result = await _misDbContext.BackboneRouterSwitchInformation.FromSqlRaw(query_str).FirstOrDefaultAsync();
            return result == null ? new BackboneRouterSwitchInformationViewModel() : result;
        }

        public async Task<List<PackagePlanViewModel>> GetAllPackageNameList(int distributorID)
        {
            var query_str = $"select PackageID, PackageName  from tbl_PackagePlan where PackageID in (select distinct PackageID from Kh_IpAddressRadius where DistributorID = {distributorID}) and Status = 1";
            var result = await _misDbContext.PackagePlanList.FromSqlRaw(query_str).ToListAsync();
            return result;
        }

        public async Task<List<PoolNameListModel>> GetBtsWisePoolNameList(string customerType, int btsId, int distributorId, int packageId)
        {
            var query_str = string.Empty;

            if (customerType == "static")
            {
                query_str = $"select distinct PoolName, VLAN from Kh_IpAddress where BTSID='{Convert.ToInt32(btsId)}' order by PoolName";
            }
            else if(customerType == "radius")
            {
                if (distributorId == 0)
                {
                    throw new Exception("Please select distributor");
                }
                if (packageId == 0)
                {
                    throw new Exception("Please select package");
                }
                query_str = $"select distinct PoolName, VLAN from Kh_IpAddressRadius where BTSID = {btsId} " +
                    $"and DistributorID = {distributorId} and PackageID = {packageId} order by PoolName";
                //query_str = $"select distinct HostName from Kh_IpAddressRadius where BTSID='{Convert.ToInt32(btsId)}' order by HostName";
            }
            else
            {
                throw new Exception("Please select your customer type");
            }
            
            var result = await _misDbContext.PoolNameList.FromSqlRaw(query_str).ToListAsync();
            return result;
        }
    }
}
