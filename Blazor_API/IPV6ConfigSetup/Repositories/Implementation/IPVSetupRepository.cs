using IPV6ConfigSetup.DataAccess;
using IPV6ConfigSetup.DataAccess.Model;
using IPV6ConfigSetup.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IPV6ConfigSetup.Repositories.Implementation
{
    public class IPVSetupRepository : IIPVSetupRepository
    {
        private readonly IPV6ConfigSetupDBContext _iPVDbContext;
        private readonly ILogger<IPVSetupRepository> _logger;
        public IPVSetupRepository(IPV6ConfigSetupDBContext iPVDbContext, ILogger<IPVSetupRepository> logger)
        {
            _iPVDbContext = iPVDbContext;
            _logger = logger;
        }


        public async Task InsertPrimarySubnet(IPV6_PrimarySubnetModel primaryModel)
        {
            var getAPrimarySubnet = await _iPVDbContext.IPV6_PrimarySubnet.FirstOrDefaultAsync(x => x.PrimarySubnet == primaryModel.PrimarySubnet);
            if (getAPrimarySubnet != null)
            {
                throw new Exception("Duplicate Data");
            }
            primaryModel.CreatedAt = DateTime.UtcNow;
            primaryModel.CreatedBy = primaryModel.CreatedBy;
            primaryModel.IsActive = 1;
            primaryModel.IsDeleted = false;

            await _iPVDbContext.IPV6_PrimarySubnet.AddAsync(primaryModel);
            await _iPVDbContext.SaveChangesAsync();
        }

        public async Task<List<IPV6_PrimarySubnetModel>> GetAllPrimarySubnet()
        {
            var getAllPrimarySubnet = await _iPVDbContext.IPV6_PrimarySubnet.Where(x => x.IsActive == 1).ToListAsync();
            return getAllPrimarySubnet;
        }

        public async Task InsertDivisionSubnet(List<IPV6_DivisionSubnet32Model> divisionModel)
        {
            await _iPVDbContext.IPV6_DivisionSubnet32.AddRangeAsync(divisionModel);
            await _iPVDbContext.SaveChangesAsync();
        }

        public async Task<List<IPV6_DivisionSubnet32Model>> GetAllDivisionSubnet(long primarySubnetId)
        {
            var getAllDivisionSubnet = new List<IPV6_DivisionSubnet32Model>();
            if (primarySubnetId == 0)
            {
                getAllDivisionSubnet = await _iPVDbContext.IPV6_DivisionSubnet32.Where(x => x.IsActive == 1).ToListAsync();
            }
            else
            {
                getAllDivisionSubnet = await _iPVDbContext.IPV6_DivisionSubnet32.Where(x => x.IPV6_PrimarySubnetId == primarySubnetId && x.IsActive == 1).ToListAsync();
            }
            
            return getAllDivisionSubnet;
        }

        public async Task InsertUserTypeSubnet(List<IPV6_UserTypeSubnet36Model> userTypeModel)
        {
            await _iPVDbContext.IPV6_UserTypeSubnet36.AddRangeAsync(userTypeModel);
            await _iPVDbContext.SaveChangesAsync();
        }

        public async Task<List<IPV6_UserTypeSubnet36Model>> GetAllUserTypeSubnet(long DivisionSubnetId)
        {
            var getAllUserTypeSubnet = new List<IPV6_UserTypeSubnet36Model>();
            if (DivisionSubnetId == 0)
            {
                getAllUserTypeSubnet = await _iPVDbContext.IPV6_UserTypeSubnet36.Where(x => x.IsActive == 1).ToListAsync();
            }
            else
            {
                getAllUserTypeSubnet = await _iPVDbContext.IPV6_UserTypeSubnet36.Where(x => x.IPV6_DivisionSubnetId == DivisionSubnetId && x.IsActive == 1).ToListAsync();
            }
            return getAllUserTypeSubnet;
        }

        public async Task InsertCitySubnet(List<IPV6_CitySubnet44Model> cityModel)
        {
            await _iPVDbContext.IPV6_CitySubnet44.AddRangeAsync(cityModel);
            await _iPVDbContext.SaveChangesAsync();
        }

        public async Task<List<IPV6_CitySubnet44Model>> GetAllCitySubnet(long userTypeSubnetId)
        {
            var getAllCitySubnet =  new List<IPV6_CitySubnet44Model>();
            if (userTypeSubnetId == 0)
            {
                var lastCitySubnetId = await _iPVDbContext.IPV6_BTSSubnet48.FromSqlRaw("SELECT top 1 * FROM [dbo].[IPV6_BTSSubnet48] where id > 1048576 ORDER BY Id desc").ToListAsync();
                var setId = lastCitySubnetId.Count == 0 ? 0 : lastCitySubnetId[0].IPV6_CitySubnetId;
                getAllCitySubnet = await _iPVDbContext.IPV6_CitySubnet44.Where(x => x.IsActive == 1 && x.Id > setId).ToListAsync();
            }
            else
            {
                getAllCitySubnet = await _iPVDbContext.IPV6_CitySubnet44.Where(x => x.IPV6_UserTypeSubnetId == userTypeSubnetId && x.IsActive == 1).ToListAsync();
            }
            return getAllCitySubnet;
        }

        public async Task InsertBTSSubnet(List<IPV6_BTSSubnet48Model> btsModel)
        {
            await _iPVDbContext.IPV6_BTSSubnet48.AddRangeAsync(btsModel);
            await _iPVDbContext.SaveChangesAsync();
        }

        public async Task<long> LastCitySubnetIdFormBTSSubnetTable()
        {
            var lastCitySubnetId = await _iPVDbContext.IPV6_BTSSubnet48.FromSqlRaw("SELECT top 1 * FROM [dbo].[IPV6_BTSSubnet48] ORDER BY Id desc").ToListAsync();
            return lastCitySubnetId.Count == 0 ? 0 : lastCitySubnetId[0].IPV6_CitySubnetId;
        }

        public async Task<List<IPV6_BTSSubnet48Model>> GetAllBTSSubnet(long citySubnetId)
        {
            var getAllBTSSubnet = new List<IPV6_BTSSubnet48Model>();
            if (citySubnetId == 0)
            {
                var lastBTSSubnetId = await _iPVDbContext.IPV6_ParentSubnet56.FromSqlRaw("SELECT top 1 * FROM [dbo].[IPV6_ParentSubnet56] where id > 1048576 ORDER BY Id desc").ToListAsync();
                var setId = lastBTSSubnetId.Count == 0 ? 0 : lastBTSSubnetId[0].IPV6_BTSSubnetId;
                getAllBTSSubnet = await _iPVDbContext.IPV6_BTSSubnet48.Where(x => x.IsActive == 1 && x.Id > setId).Take(10).ToListAsync();
            }
            else
            {
                getAllBTSSubnet = await _iPVDbContext.IPV6_BTSSubnet48.Where(x => x.IPV6_CitySubnetId == citySubnetId && x.IsActive == 1).ToListAsync();
            }
            return getAllBTSSubnet;
        }

        public async Task<bool> InsertParentSubnet(List<IPV6_ParentSubnet56Model> parentModel)
        {
            await _iPVDbContext.IPV6_ParentSubnet56.AddRangeAsync(parentModel);
            return await _iPVDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<IPV6_ParentSubnet56Model>> GetAllParentSubnet(long btsSubnetId)
        {
            var getAllParentSubnet = new List<IPV6_ParentSubnet56Model>();
            if (btsSubnetId == 0)
            {
                var lastParentSubnetId = await _iPVDbContext.IPV6_CustomerSubnet64.FromSqlRaw("SELECT top 1 * FROM [dbo].[IPV6_CustomerSubnet64] ORDER BY Id desc").ToListAsync();
                var setId = lastParentSubnetId.Count == 0 ? 0 : lastParentSubnetId[0].IPV6_ParentSubnetId;
                getAllParentSubnet = await _iPVDbContext.IPV6_ParentSubnet56.Where(x => x.IsActive == 1 && x.Id > setId).Take(10).ToListAsync();
            }
            else
            {
                getAllParentSubnet = await _iPVDbContext.IPV6_ParentSubnet56.Where(x => x.IPV6_BTSSubnetId == btsSubnetId && x.IsActive == 1).ToListAsync();
            }
            return getAllParentSubnet;
        }

        public async Task<long> LastBTSSubnetIdFormParentSubnetTable()
        {
            var lastCitySubnetId = await _iPVDbContext.IPV6_ParentSubnet56.FromSqlRaw("SELECT top 1 * FROM [dbo].[IPV6_ParentSubnet56] ORDER BY Id desc").ToListAsync();
            return lastCitySubnetId.Count == 0 ? 0 : lastCitySubnetId[0].IPV6_BTSSubnetId;
        }

        public async Task<bool> InsertCustomerSubnet(List<IPV6_CustomerSubnet64Model> customerModel)
        {
            await _iPVDbContext.IPV6_CustomerSubnet64.AddRangeAsync(customerModel);
            return await _iPVDbContext.SaveChangesAsync() > 0;
        }

        public async Task<long> LastPatentSubnetIdFormCustomerSubnetTable()
        {
            var lastParentSubnetId = await _iPVDbContext.IPV6_CustomerSubnet64.FromSqlRaw("SELECT top 1 * FROM [dbo].[IPV6_CustomerSubnet64] ORDER BY Id desc").ToListAsync();
            return lastParentSubnetId.Count == 0 ? 0 : lastParentSubnetId[0].IPV6_ParentSubnetId;
        }

        public async Task<List<IPV6_CustomerSubnet64Model>> GetAllCustomerSubnet(long parentSubnetId)
        {
            var getAllCustomerSubnet = await _iPVDbContext.IPV6_CustomerSubnet64.Where(x => x.IPV6_ParentSubnetId == parentSubnetId).ToListAsync();
            return getAllCustomerSubnet;
        }
        public async Task<bool> InsertIPV6CustomerSubnet(IPV6SetupRequestModel model)
        {
            var haveParentSubnet = await _iPVDbContext.IPV6_ParentSubnet56.FirstOrDefaultAsync(x => x.IsActive == 1 && x.ParentSubnet == model.ParentSubnet);
            if(haveParentSubnet != null)
            {
                throw new Exception("Duplicate parent subnet");
            }
            using var transaction = _iPVDbContext.Database.BeginTransaction();

            try
            {
                List<IPV6_ParentSubnet56Model> parentModel = new List<IPV6_ParentSubnet56Model>();
                IPV6_ParentSubnet56Model aParentSubnet = new IPV6_ParentSubnet56Model()
                {
                    ParentSubnet = model.ParentSubnet,
                    IPV6_BTSSubnetId = 0,
                    IsActive = 1,
                    CustomerType = model.CustomerType,
                    DistributorId = model.DistributorId,
                    DistributorName = model.DistributorName,
                    PackageName = model.PackageName,
                    BTSName = model.BTSName,
                    RouterNameId = model.RouterNameId,
                    RouterName = model.RouterName,
                    RouterHostName = model.RouterHostName,
                    HostName = model.HostName,
                    RouterSwitchIP = model.RouterSwitchIP,
                    Noofporf = model.Noofporf,
                    PoolName = model.PoolName,
                    VLAN = model.VLAN,
                    Remarkes = model.Remarkes,
                    BTSId = model.BTSId,
                    PoolId = model.PoolId,
                    Gateway = model.Gateway,
                    PackageId = model.PackageId,
                    CreatedAt = model.CreatedAt,
                    CreatedBy = model.CreatedBy
                };
                parentModel.Add(aParentSubnet);
                if (await InsertParentSubnet(parentModel))
                {
                    var i = 0;
                    foreach (var item in model.IPV6_CustomerSubnets)
                    {
                        if(i == 0)
                        {
                            item.IsUsed = 1;
                            item.SubscriberID = "System";
                            item.LastModifiedAt = DateTime.UtcNow;
                            item.LastModifiedBy = "System";
                        }
                        item.IPV6_ParentSubnetId = (long)aParentSubnet.Id;
                        i++;
                    }

                    if (await InsertCustomerSubnet(model.IPV6_CustomerSubnets))
                    {
                        transaction.Commit();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                throw new Exception(ex.Message);
            }
            
        }


    }
}
