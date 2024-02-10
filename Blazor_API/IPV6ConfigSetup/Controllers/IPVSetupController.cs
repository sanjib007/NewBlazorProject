using IPV6ConfigSetup.DataAccess.CommonModel;
using IPV6ConfigSetup.DataAccess.Model;
using IPV6ConfigSetup.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPV6ConfigSetup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPVSetupController : CustomerBaseController
    {
        private readonly IIPVSetupService _IpvSetupService;
        public IPVSetupController(IIPVSetupService ipvSetupService)
        {
            _IpvSetupService = ipvSetupService;
        }

        [HttpPost("InsertPrimarySubnet")]
        public async Task<IActionResult> InsertPrimarySubnet(IPV6_PrimarySubnetModel primaryModel)
        {
            return await responseCheck(await _IpvSetupService.InsertPrimarySubnet(primaryModel));
        }

        [HttpGet("GetAllPrimarySubnet")]
        public async Task<IActionResult> GetAllPrimarySubnet()
        {
            return await responseCheck(await _IpvSetupService.GetAllPrimarySubnet()); 
        }

        [HttpPost("InsertDivisionSubnet")]
        public async Task<IActionResult> InsertDivisionSubnet(List<IPV6_DivisionSubnet32Model> divisionModel)
        {
            return await responseCheck(await _IpvSetupService.InsertDivisionSubnet(divisionModel));
        }

        [HttpGet("GetAllDivisionSubnet")]
        public async Task<IActionResult> GetAllDivisionSubnet(long primarySubnetId)
        {
            return await responseCheck(await _IpvSetupService.GetAllDivisionSubnet(primarySubnetId));
        }

        [HttpGet("GetAllDivisionSubnetById/{primarySubnetId}")]
        public async Task<IActionResult> GetAllDivisionSubnetById(long primarySubnetId)
        {
            return await responseCheck(await _IpvSetupService.GetAllDivisionSubnet(primarySubnetId));
        }

        [HttpPost("InsertUserTypeSubnet")]
        public async Task<IActionResult> InsertUserTypeSubnet(List<IPV6_UserTypeSubnet36Model> userTypeModel)
        {
            return await responseCheck(await _IpvSetupService.InsertUserTypeSubnet(userTypeModel));
        }

        [HttpGet("GetAllUserTypeSubnet")]
        public async Task<IActionResult> GetAllUserTypeSubnet()
        {
            return await responseCheck(await _IpvSetupService.GetAllUserTypeSubnet());
        }

        [HttpGet("GetAllUserTypeSubnetById/{DivisionSubnetId}")]
        public async Task<IActionResult> GetAllUserTypeSubnetById(long DivisionSubnetId)
        {
            return await responseCheck(await _IpvSetupService.GetAllUserTypeSubnet(DivisionSubnetId));
        }

        [HttpPost("InsertCitySubnet")]
        public async Task<IActionResult> InsertCitySubnet(List<IPV6_CitySubnet44Model> cityModel)
        {
            return await responseCheck(await _IpvSetupService.InsertCitySubnet(cityModel));
        }

        [HttpGet("GetAllCitySubnet")]
        public async Task<IActionResult> GetAllCitySubnet()
        {
            return await responseCheck(await _IpvSetupService.GetAllCitySubnet());
        }

        [HttpGet("GetAllCitySubnetById/{userTypeSubnetId}")]
        public async Task<IActionResult> GetAllCitySubnetById(long userTypeSubnetId)
        {
            return await responseCheck(await _IpvSetupService.GetAllCitySubnet(userTypeSubnetId));
        }

        [HttpPost("InsertBTSSubnet")]
        public async Task<IActionResult> InsertBTSSubnet(List<IPV6_BTSSubnet48Model> btsModel)
        {
            return await responseCheck(await _IpvSetupService.InsertBTSSubnet(btsModel));
        }

        [HttpGet("GetAllBTSSubnet")]
        public async Task<IActionResult> GetAllBTSSubnet()
        {
            return await responseCheck(await _IpvSetupService.GetAllBTSSubnet());
        }

        [HttpGet("GetAllBTSSubnetById/{citySubnetId}")]
        public async Task<IActionResult> GetAllBTSSubnetById(long citySubnetId)
        {
            return await responseCheck(await _IpvSetupService.GetAllBTSSubnet(citySubnetId));
        }

        [HttpPost("InsertParentSubnet")]
        public async Task<IActionResult> InsertParentSubnet(List<IPV6_ParentSubnet56Model> parentModel)
        {
            return await responseCheck(await _IpvSetupService.InsertParentSubnet(parentModel));
        }

        [HttpGet("GetAllParentSubnet")]
        public async Task<IActionResult> GetAllParentSubnet()
        {
            return await responseCheck(await _IpvSetupService.GetAllParentSubnet());
        }

        [HttpGet("GetAllParentSubnetById/{btsSubnetId}")]
        public async Task<IActionResult> GetAllParentSubnetById(long btsSubnetId)
        {
            return await responseCheck(await _IpvSetupService.GetAllParentSubnet(btsSubnetId));
        }

        [HttpPost("InsertCustomerSubnet")]
        public async Task<IActionResult> InsertCustomerSubnet(List<IPV6_CustomerSubnet64Model> customerModel)
        {
            return await responseCheck(await _IpvSetupService.InsertCustomerSubnet(customerModel));
        }

        [HttpGet("GetAllCustomerSubnet/{parentSubnetId}")]
        public async Task<IActionResult> GetAllCustomerSubnet(long parentSubnetId)
        {
            return await responseCheck(await _IpvSetupService.GetAllCustomerSubnet(parentSubnetId));
        }

        [HttpPost("InsertIPV6CustomerSubnet")]
        public async Task<IActionResult> InsertIPV6CustomerSubnet(IPV6SetupRequestModel model)
        {
            return await responseCheck(await _IpvSetupService.InsertIPV6CustomerSubnet(model));
        }
    }
}
