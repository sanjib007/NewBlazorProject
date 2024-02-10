using IPV6ConfigSetup.DataAccess.CommonModel;
using IPV6ConfigSetup.DataAccess.Model;
using IPV6ConfigSetup.Repositories.Interface;
using IPV6ConfigSetup.Services.Interface;

namespace IPV6ConfigSetup.Services.Implementation
{
    public class IPVSetupService : IIPVSetupService
    {
        private readonly IIPVSetupRepository _repository;
        private readonly ILogger<IPVSetupService> _logger;
        public IPVSetupService(IIPVSetupRepository repository, ILogger<IPVSetupService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        private async Task<ApiResponse> SuccessMethod(object model, string message)
        {
            return new ApiResponse()
            {
                Status = "Success",
                StatusCode = 200,
                Message = message,
                Data = model
            };
        }

        private async Task<ApiResponse> ErrorMethod(object model, string message)
        {
            return new ApiResponse()
            {
                Status = "Error",
                StatusCode = 400,
                Message = message,
                Data = model
            };
        }

        public async Task<ApiResponse> InsertPrimarySubnet(IPV6_PrimarySubnetModel primaryModel)
        {
            try
            {
                await _repository.InsertPrimarySubnet(primaryModel);
                return await SuccessMethod(null, "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
            
        }

        public async Task<ApiResponse> GetAllPrimarySubnet()
        {
            try
            {
                return await SuccessMethod(await _repository.GetAllPrimarySubnet(), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> InsertDivisionSubnet(List<IPV6_DivisionSubnet32Model> divisionModel)
        {            
            try
            {
                await _repository.InsertDivisionSubnet(divisionModel);
                return await SuccessMethod(null, "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllDivisionSubnet(long primarySubnetId)
        {
            try
            {
                return await SuccessMethod(await _repository.GetAllDivisionSubnet(primarySubnetId), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> InsertUserTypeSubnet(List<IPV6_UserTypeSubnet36Model> userTypeModel)
        {
            
            try
            {
                await _repository.InsertUserTypeSubnet(userTypeModel);
                return await SuccessMethod(null, "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllUserTypeSubnet(long DivisionSubnetId)
        {
            try
            {
                return await SuccessMethod(await _repository.GetAllUserTypeSubnet(DivisionSubnetId), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> InsertCitySubnet(List<IPV6_CitySubnet44Model> cityModel)
        {
            try
            {
                await _repository.InsertCitySubnet(cityModel);
                return await SuccessMethod(null, "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllCitySubnet(long userTypeSubnetId)
        {
            try
            {
                var lastCitySubnetIdFromBTSSubnetTable = await _repository.LastCitySubnetIdFormBTSSubnetTable();
                return await SuccessMethod(await _repository.GetAllCitySubnet(userTypeSubnetId), lastCitySubnetIdFromBTSSubnetTable.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> InsertBTSSubnet(List<IPV6_BTSSubnet48Model> btsModel)
        {
            try
            {
                await _repository.InsertBTSSubnet(btsModel);
                return await SuccessMethod(null, "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllBTSSubnet(long citySubnetId)
        {
            try
            {
                var lastBTSSubnetIdFromPatentSubnetTable = await _repository.LastBTSSubnetIdFormParentSubnetTable();
                return await SuccessMethod(await _repository.GetAllBTSSubnet(citySubnetId), lastBTSSubnetIdFromPatentSubnetTable.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> InsertParentSubnet(List<IPV6_ParentSubnet56Model> parentModel)
        {
            try
            {
                await _repository.InsertParentSubnet(parentModel);
                return await SuccessMethod(null, "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllParentSubnet(long btsSubnetId)
        {
            try
            {
                var lastParentSubnetIdFromCustomerSubnetTable = await _repository.LastPatentSubnetIdFormCustomerSubnetTable();
                return await SuccessMethod(await _repository.GetAllParentSubnet(btsSubnetId), lastParentSubnetIdFromCustomerSubnetTable.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> InsertCustomerSubnet(List<IPV6_CustomerSubnet64Model> customerModel)
        {
            try
            {
                await _repository.InsertCustomerSubnet(customerModel);
                return await SuccessMethod(null, "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllCustomerSubnet(long parentSubnetId)
        {
            try
            {
                return await SuccessMethod(await _repository.GetAllCustomerSubnet(parentSubnetId), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> InsertIPV6CustomerSubnet(IPV6SetupRequestModel model)
        {
            try
            {
                
                return await SuccessMethod(await _repository.InsertIPV6CustomerSubnet(model), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }
    }
}
