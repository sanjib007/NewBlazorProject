using IPV6ConfigSetup.Services.Implementation;
using IPV6ConfigSetup.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPV6ConfigSetup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MisController : CustomerBaseController
    {
        private readonly IMisDBService _misdbService;
        public MisController(IMisDBService misdbService)
        {
            _misdbService = misdbService;
        }

        [HttpGet("GetAllDistributorList")]
        public async Task<IActionResult> GetAllDistributorList()
        {
            return await responseCheck(await _misdbService.GetAllDistributorList());
        }

        [HttpGet("GetAllBtsList/{customerType}/{distributorID}/{packageID}")]
        public async Task<IActionResult> GetAllBtsList(string customerType, int distributorID = 0, int packageID = 0)
        {
            return await responseCheck(await _misdbService.GetAllBtsList(customerType, distributorID, packageID));
        }

        [HttpGet("GetBtsWisePoolNameList/{customerType}/{btsId}/{distributorId}/{packageId}")]
        public async Task<IActionResult> GetBtsWisePoolNameList(string customerType, int btsId, int distributorId = 0, int packageId = 0)
        {
            return await responseCheck(await _misdbService.GetBtsWisePoolNameList(customerType, btsId, distributorId, packageId));
        }

        [HttpGet("GetAllRouterNameList/{btsId}")]
        public async Task<IActionResult> GetAllRouterNameList(int btsId)
        {
            return await responseCheck(await _misdbService.GetAllRouterNameList(btsId));
        }

        [HttpGet("GetRouterInformation/{btsId}/{routerId}")]
        public async Task<IActionResult> GetRouterInformation(int btsId, string routerId)
        {
            return await responseCheck(await _misdbService.GetRouterInformation(btsId, routerId));
        }

        [HttpGet("GetAllPackageNameList/{distributorID}")]
        public async Task<IActionResult> GetAllPackageNameList(int distributorID)
        {
            return await responseCheck(await _misdbService.GetAllPackageNameList(distributorID));
        }
    }
}
