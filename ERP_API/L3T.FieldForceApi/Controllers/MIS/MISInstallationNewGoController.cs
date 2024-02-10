using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.FieldForceApi.Controllers.MIS
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class MISInstallationNewGoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MISInstallationNewGoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<string> GetClientIPAddress()
        {
            var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var localIpAddress = httpConnectionFeature?.LocalIpAddress;
            var remoteIpAddress = httpConnectionFeature?.RemoteIpAddress;
            var ip = localIpAddress.ToString() + "/" + remoteIpAddress.ToString();
            return ip;
        }

        [HttpGet("GetParentDataForNewGoMis")]
        public async Task<IActionResult> GetParentDataForNewGoMis(string? userId, string? ticketId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new LoadParentDataQuery()
                {
                    user = user,
                    ip = ip,
                    userId = userId,
                    ticketId = ticketId
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetCustomerInformationForNewGoMis")]
        public async Task<IActionResult> GetCustomerInformationForNewGoMis(string? subscriberId, int? serialNo)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ClientInformationFornewGoQuery()
                {
                    user = user,
                    ip = ip,
                    subscriberId = subscriberId,
                    serialNo = (int)serialNo
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetP2PAddressForNewGoMis")]
        public async Task<IActionResult> GetP2PAddressForNewGoMis(string? subscriberId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new P2PAddressForNewGoQuery()
                {
                    user = user,
                    ip = ip,
                    subscriberId = subscriberId
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetPostInstallationDataByServiceId")]
        public async Task<IActionResult> GetPostInstallationDataByServiceId(string? ticketId, int? serviceId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new PostInstallationDataByServiceIdQuery()
                {
                    user = user,
                    ip = ip,
                    ticketId = ticketId,
                    serviceId = (int)serviceId
                });
                return Ok(meidatorObj);
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



        [HttpGet("GetAllServiceDataforNewGoMis")]
        public async Task<IActionResult> GetAllServiceDataforNewGoMis(string? ticketId, string? serviceId,
            string? brClientCode, int? brSerialNumber, int? btsId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new AllServiceDataForNewGoQuery()
                {
                    user = user,
                    ip = ip,
                    ticketId = ticketId,
                    serviceId = serviceId,
                    brClientCode = brClientCode,
                    brSerialNumber = (int)brSerialNumber,
                    btsId = (int)btsId
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetServiceWisePermissionInfo")]
        public async Task<IActionResult> GetServiceWisePermissionInfo(string? userId, string? ticketId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ServiceWisePermissionQuery()
                {
                    userId = userId,
                    ip = ip,
                    ticketId = ticketId
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetEmployeeTicketPriority")]
        public async Task<IActionResult> GetEmployeeTicketPriority(string? userId, string? ticketId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new EmployeeTicketPriorityQuery()
                {
                    userId = userId,
                    ip = ip,
                    ticketId = ticketId
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetConnectivityTrayList")]
        public async Task<IActionResult> GetConnectivityTrayList()
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ConnectivityTrayListQuery()
                {
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetConnectivityPortList")]
        public async Task<IActionResult> GetConnectivityPortList()
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ConnectivityPortListQuery()
                {
                    user = user,
                    ip = ip
                });
                return Ok(meidatorObj);
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


        [HttpGet("LoadColorDetails")]
        public async Task<IActionResult> LoadColorDetails(string? brClientCode, int? brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new ColorRelatedDetailsDataQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = (int)brSerialNumber
                });
                return Ok(meidatorObj);
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


        [HttpGet("P2PFiberDetails")]
        public async Task<IActionResult> P2PFiberDetails(string? brClientCode, int? brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new P2PFiberDetailsDataQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = (int)brSerialNumber
                });
                return Ok(meidatorObj);
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

        [HttpGet("DarkFiberClientInformation")]
        public async Task<IActionResult> DarkFiberClientInformation(string? brClientCode, int? brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new DarkClientInformationQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = (int)brSerialNumber
                });
                return Ok(meidatorObj);
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

        [HttpGet("DarkFiberClientColorInformation")]
        public async Task<IActionResult> DarkFiberClientColorInformation(string? brClientCode, int? brSerialNumber, int? noOfCore)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new DarkFiberClientColorInformationQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = (int)brSerialNumber,
                    noOfCore = (int)noOfCore
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetSMSDataForNewGo")]
        public async Task<IActionResult> GetSMSDataForNewGo(string? ticketId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetSMSDataForNewGoQuery()
                {
                    user = user,
                    ip = ip,
                    ticketId = ticketId
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetPaymentDataForNewGo")]
        public async Task<IActionResult> GetPaymentDataForNewGo(string? ticketId)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetPaymentDataForNewGoQuery()
                {
                    user = user,
                    ip = ip,
                    ticketId = ticketId
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetIpTvInfo")]
        public async Task<IActionResult> GetIpTvInfo(string? brClientCode)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetIpTvInfoQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetPacakgeNameInfo")]
        public async Task<IActionResult> GetPAcakgeNameInfo(string? brClientCode, int? brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetPAcakgeNameInfoQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = (int)brSerialNumber
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetHost_IPInfo")]
        public async Task<IActionResult> GetHost_IPInfo(string? brClientCode, int? brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetHost_IPInfoQuery()
                {
                    user = user,
                    ip = ip,
                    brClientCode = brClientCode,
                    brSerialNumber = (int)brSerialNumber
                });
                return Ok(meidatorObj);
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

        [HttpGet("GetKh_IpAddressByHostName")]
        public async Task<IActionResult> GetKh_IpAddressByHostName(string? hostName)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetKh_IpAddressByHostNameQuery()
                {
                    user = user,
                    ip = ip,
                    hostName = hostName
                });
                return Ok(meidatorObj);
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

        [HttpPost("UpdateP2MDataForTicketClose")]
        public async Task<IActionResult> UpdateP2MDataForTicketClose(string? splitterName, string? fiberoltbrand, string? fiberoltname,
            int? fiberpon, int? fiberport, int? portcapfiber, string? encloserno, string? refnoOrTicketId, string? branchidOrCliCode,
            int? slnoOrCustomerCodeSlNo, string? customerName, string? customerBranchName, string? customerAddressline1, int? btsSetupId,
            string? fiberLaser, string? btsName, int? cableNumberFiber, string? linkPathFiber, string? remarksFiber, string? emailBody
            )
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new UpdateP2MDataForTicketCloseCommand()
                {
                    user = user,
                    ip = ip,
                    splitterNameFiber = splitterName,
                    fiberoltbrand = fiberoltbrand,
                    fiberoltname = fiberoltname,
                    fiberpon = (int)fiberpon,
                    fiberport = (int)fiberport,
                    portcapfiber = (int)portcapfiber,
                    encloserno = encloserno,
                    refnoOrTicketId = refnoOrTicketId,
                    branchidOrCliCode = branchidOrCliCode,
                    slnoOrCustomerCodeSlNo = (int)slnoOrCustomerCodeSlNo,
                    customerName = customerName,
                    customerBranchName = customerBranchName,
                    customerAddressline1 = customerAddressline1,
                    btsSetupId = (int)btsSetupId,
                    fiberLaser = fiberLaser,
                    btsName = btsName,
                    cableNumber = (int)cableNumberFiber,
                    linkPathFiber = linkPathFiber,
                    remarksFiber = remarksFiber,
                    emailBody = emailBody

                });
                return Ok(meidatorObj);
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

        [HttpPost("DoneP2MDataForTicketClose")]
        public async Task<IActionResult> DoneP2MDataForTicketClose(MisInstP2MTicketCloseRequestModel requestModel)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new DoneP2MDataForTicketCloseCommand()
                {
                    user = user,
                    ip = ip,
                    requestModel = requestModel
                });
                return Ok(meidatorObj);
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
