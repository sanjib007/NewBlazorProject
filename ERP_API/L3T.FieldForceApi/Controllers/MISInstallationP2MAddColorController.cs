using L3T.FieldForceApi.CommandQuery.Command;
using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace L3T.FieldForceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "Admin")]
    public class MISInstallationP2MAddColorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MISInstallationP2MAddColorController(IMediator mediator)
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

        [HttpGet("GetColorInfoCommon")]
        public async Task<IActionResult> GetColorInfoCommon()
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetColorInfoQuery()
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

        [HttpGet("GetP2MCrUploadFileInfo")]
        //public async Task<IActionResult> GetP2MCrUploadFileInfo(int? autoODFID)
        public async Task<IActionResult> GetP2MCrUploadFileInfo(int? slnoOrCustomerCodeSlNo)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetP2MCrUploadFileQuery()
                {
                    user = user,
                    ip = ip,
                    autoODFID = (int)slnoOrCustomerCodeSlNo
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

        [HttpPost("AddToODF_JoincolorEntry")]
        public async Task<IActionResult> AddToODF_JoincolorEntry(tbl_ODF_JoincolorEntryRequestModel requestModel)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new AddTotbl_ODF_JoincolorEntryCommand()
                {
                    user = user,
                    ip = ip,
                    model = requestModel
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

        [HttpGet("GetAllSpliterInfo")]
        public async Task<IActionResult> GetAllSpliterInfo(string? prefixText, int? count, string? btsId)
        //public async Task<IActionResult> GetAllSpliterInfo(L3t, 18, 20)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetAllSpliterInfoQuery()
                {
                    user = user,
                    ip = ip,
                    prefixText = prefixText,
                    count = (int)count,
                    btsId = btsId
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

        [HttpGet("GetAllCableTypeInfo")]
        public async Task<IActionResult> GetAllCableTypeInfo()
        {
            var listOfCableType = new List<string>();
            var response = new ApiResponse();

            listOfCableType.Add("96F");
            listOfCableType.Add("48F");
            listOfCableType.Add("24F");
            listOfCableType.Add("12F");
            listOfCableType.Add("6F");
            listOfCableType.Add("4F");
            listOfCableType.Add("2F");
            response = new ApiResponse()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Data get successfully",
                Data = listOfCableType
            };
            return Ok(response);
        }

        [HttpGet("GetCableNoFiber")]
        public async Task<IActionResult> GetCableNoFiber()
        {
            var response = new ApiResponse();
            var listOfCableNoFiber = new List<int>();
            for (int i = 1; i <= 52; i++)
            {
                listOfCableNoFiber.Add(i);
            }
            response = new ApiResponse()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Data get successfully",
                Data = listOfCableNoFiber
            };
            return Ok(response);
        }

        [HttpGet("GetDataLogByIpComment")]
        public async Task<IActionResult> GetDataLogByIpComment(string? ipComment, string? ticketId, string? teamName)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetDataLogByIpCommentQuery()
                {
                    user = user,
                    ip = ip,
                    ipComment = ipComment,
                    ticketId = ticketId,
                    teamName = teamName
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


        //[HttpPost("UpdateP2MDataForTicketClose")]
        //public async Task<IActionResult> UpdateP2MDataForTicketClose(string? splitterName, string? fiberoltbrand, string? fiberoltname,
        //    int? fiberpon, int? fiberport, int? portcapfiber, string? encloserno, string? refnoOrTicketId, string? branchidOrCliCode,
        //    int? slnoOrCustomerCodeSlNo, string? customerName, string? customerBranchName, string? customerAddressline1, int? btsSetupId,
        //    string? fiberLaser, string? btsName, int? cableNumber, string? linkPathFiber, string? remarksFiber, string? emailBody
        //    )
        //{
        //    var ip = await GetClientIPAddress();
        //    var user = User;
        //    try
        //    {
        //        var meidatorObj = await _mediator.Send(new UpdateP2MDataForTicketCloseCommand()
        //        {
        //            user = user,
        //            ip = ip,
        //            splitterNameFiber = splitterName,
        //            fiberoltbrand = fiberoltbrand,
        //            fiberoltname = fiberoltname,
        //            fiberpon = (int)fiberpon,
        //            fiberport = (int)fiberport,
        //            portcapfiber = (int)portcapfiber,
        //            encloserno = encloserno,
        //            refnoOrTicketId = refnoOrTicketId,
        //            branchidOrCliCode = branchidOrCliCode,
        //            slnoOrCustomerCodeSlNo = (int)slnoOrCustomerCodeSlNo,
        //            customerName = customerName,
        //            customerBranchName = customerBranchName,
        //            customerAddressline1 = customerAddressline1,
        //            btsSetupId = (int)btsSetupId,
        //            fiberLaser = fiberLaser,
        //            btsName = btsName,
        //            cableNumber = (int)cableNumber,
        //            linkPathFiber = linkPathFiber,
        //            remarksFiber = remarksFiber,
        //            emailBody = emailBody

        //        });
        //        return Ok(meidatorObj);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse()
        //        {
        //            Status = "Error",
        //            StatusCode = 400,
        //            Message = ex.Message
        //        });
        //    }


        //}

    }
}
