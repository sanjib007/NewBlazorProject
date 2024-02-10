using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.PPPoE;
using L3T.Infrastructure.Helpers.Models.PPPoE.RequestModel;
using L3T.Infrastructure.Helpers.Models.SelfCare.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using OpenIddict.Validation.AspNetCore;
using System.Data;
using System.Data.Common;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace L3T.PPPoEService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PPPoEController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly IPPPoEService _pppoeService;
        public PPPoEController(ITestService testService, IPPPoEService pppoeService)
        {
            _testService = testService;
            _pppoeService = pppoeService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> IndexTest()
        {
            try
            {

                var a = await _testService.GetTestList(5); 

                var response = new ApiResponse()
                {
                    Status = "success",
                    StatusCode = 200,
                    Message = "200",
                    Data = "Related data"
                }; 
                //return response;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("get-radcheck")]
        public async Task<IActionResult> GetRadcheck()
        {

            try
            {
                var response = await _pppoeService.GetRadcheck("khan");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("nas-create")]
        public async Task<IActionResult> NasCreate([FromBody] NasRequestModel requestModel)
        {

            string router_name = requestModel.router_name;
            string secret = requestModel.secret;
            string router_ip = requestModel.router_ip;

            try
            {
                var response = await _pppoeService.NasCreate(router_name, secret, router_ip);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("get-customer-info")]
        public async Task<IActionResult> GetCustomerInfo(string client_id)
        {
            try
            {
                var response = await _pppoeService.GetCustomerInfo(client_id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("expiry-update")]
        public async Task<IActionResult> ExpiryUpdate(string client_id, DateTime expiry_date)
        {
            try
            {
                var response = await _pppoeService.ExpiryUpdate(client_id, expiry_date);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("package-update")]
        public async Task<IActionResult> PackageUpdate(string client_id, string new_package)
        {
            try
            {
                var response = await _pppoeService.PackageUpdate(client_id, new_package);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("ippool-create")]
        public async Task<IActionResult> IpPoolCreate([FromBody] IpPoolRequestModel requestModel)
        {

            string pool_name = requestModel.pool_name;
            string first_ip = requestModel.first_ip;
            string last_ip = requestModel.last_ip;

            try
            {
                var response = await _pppoeService.IpPoolCreate(pool_name, first_ip, last_ip);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }


        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel requestModel)
        {
            string username = requestModel.username;
            string package = requestModel.package; 
            string password = requestModel.password; 
            string ip = requestModel.ip;
              
            try
            {
                var response = await _pppoeService.CreateUser(username, package, password, ip);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse()
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
           
        }
    }
}
