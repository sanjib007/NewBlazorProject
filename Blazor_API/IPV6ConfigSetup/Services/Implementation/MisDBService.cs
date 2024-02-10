using IPV6ConfigSetup.DataAccess.CommonModel;
using IPV6ConfigSetup.Repositories.Interface;
using IPV6ConfigSetup.Services.Interface;

namespace IPV6ConfigSetup.Services.Implementation
{
    public class MisDBService : IMisDBService
    {
        private readonly IMisDBRepository _misdbRepository;
        private readonly ILogger<MisDBService> _logger;
        public MisDBService(IMisDBRepository misdbRepository, ILogger<MisDBService> logger)
        {
            _misdbRepository = misdbRepository;
            _logger = logger;
        }

        public async Task<ApiResponse> GetAllDistributorList()
        {
            try
            {
                return await SuccessMethod(await _misdbRepository.GetAllDistributorList(), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllBtsList(string customerType, int distributorID, int packageID)
        {
            try
            {
                return await SuccessMethod(await _misdbRepository.GetAllBtsList(customerType, distributorID, packageID), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetBtsWisePoolNameList(string customerType, int btsId, int distributorId, int packageId)
        {
            try
            {
                return await SuccessMethod(await _misdbRepository.GetBtsWisePoolNameList(customerType, btsId, distributorId, packageId), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllRouterNameList(int btsId)
        {
            try
            {
                return await SuccessMethod(await _misdbRepository.GetAllRouterNameList(btsId), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetRouterInformation(int btsId, string routerID)
        {
            try
            {
                return await SuccessMethod(await _misdbRepository.GetRouterInformation(btsId, routerID), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllPackageNameList(int distributorID)
        {
            try
            {
                return await SuccessMethod(await _misdbRepository.GetAllPackageNameList(distributorID), "Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($@"ErrorMessage : {ex.Message} /n Error : {ex}");
                return await ErrorMethod(ex, ex.Message);
            }
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
    }
}
