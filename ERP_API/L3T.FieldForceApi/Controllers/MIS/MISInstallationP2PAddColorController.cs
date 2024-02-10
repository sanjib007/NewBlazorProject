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
    public class MISInstallationP2PAddColorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MISInstallationP2PAddColorController(IMediator mediator)
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

        [HttpGet("GetSplitterCrUploadFile")]
        public async Task<IActionResult> GetSplitterCrUploadFile(string? brClientCode, int? brSerialNumber)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetCrUploadFileQuery()
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


        [HttpPost("AddToSplitter_JoincolorEntry")]
        public async Task<IActionResult> AddToSplitter_JoincolorEntry(tbl_Splitter_JoincolorEntryRequestModel requestModel)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new AddToODF_JoincolorEntryCommand()
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

        [HttpGet("GetP2PMCTypeInfo")]
        public async Task<IActionResult> GetP2PMCTypeInfo()
        {
            var response = new ApiResponse();
            var listOfP2PMCType = new List<string>();
            listOfP2PMCType.Add("A Type(1310 nm)");
            listOfP2PMCType.Add("B Type(1550 nm)");
            listOfP2PMCType.Add("Duel Core Fiber");
            response = new ApiResponse()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Data get successfully",
                Data = listOfP2PMCType
            };
            return Ok(response);
        }



        [HttpPost("UpdateP2PDataForTicketClose")]
        public async Task<IActionResult> UpdateP2PDataForTicketClose(string? refnoOrTicketId, string? branchidOrCliCode,
            int? slnoOrCustomerCodeSlNo, string? emailBody, int? cablePathID_DDLcablnetwork, string? Typeofp2mlink_DDLtypeofp2mlinkText,
            string? p2pSwitchRouIP, string? p2pSwRouPortNew, string? p2pLaserNew, string? p2PMCTypeInfo, string? btsSetupName,
            int? btsSetupId, string? customerName, string? customerBranchName, string? customerAddressline1,
            string? linkpathp2p_GooglePath, string? remarksp2pText, int? autoOFIID_IncrementID)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new UpdateP2PDataForTicketCloseCommand()
                {
                    user = user,
                    ip = ip,
                    refnoOrTicketId = refnoOrTicketId,
                    branchidOrCliCode = branchidOrCliCode,
                    slnoOrCustomerCodeSlNo = (int)slnoOrCustomerCodeSlNo,
                    emailBody = emailBody,
                    cablePathID_DDLcablnetwork = (int)cablePathID_DDLcablnetwork,
                    Typeofp2mlink_DDLtypeofp2mlinkText = Typeofp2mlink_DDLtypeofp2mlinkText,
                    p2pSwitchRouIP = p2pSwitchRouIP,
                    p2pSwRouPortNew = p2pSwRouPortNew,
                    p2pLaserNew = p2pLaserNew,
                    p2PMCTypeInfo = p2PMCTypeInfo,
                    btsSetupName = btsSetupName,
                    btsSetupId = (int)btsSetupId,
                    customerName = customerName,
                    customerBranchName = customerBranchName,
                    customerAddressline1 = customerAddressline1,
                    linkpathp2p_GooglePath = linkpathp2p_GooglePath,
                    remarksp2pText = remarksp2pText,
                    autoOFIID_IncrementID = (int)autoOFIID_IncrementID

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

        [HttpGet("GetCableJoinInfo")]
        public async Task<IActionResult> GetCableJoinInfo()
        {
            var response = new ApiResponse();

            var listOfCableJoinInfo = new List<string>();
            listOfCableJoinInfo.Add("Tj Box");
            listOfCableJoinInfo.Add("Encloser");
            listOfCableJoinInfo.Add("ODF");

            response = new ApiResponse()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Data get successfully",
                Data = listOfCableJoinInfo
            };
            return Ok(response);
        }

        [HttpGet("GetP2PCrUploadFileInfo")]
        public async Task<IActionResult> GetP2PCrUploadFileInfo(int? autoODFID)
        {
            var ip = await GetClientIPAddress();
            var user = User;
            try
            {
                var meidatorObj = await _mediator.Send(new GetP2MCrUploadFileQuery()
                {
                    user = user,
                    ip = ip,
                    autoODFID = (int)autoODFID
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
