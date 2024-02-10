using System.Threading.Tasks;
using AutoMapper;
using CAMS.CommandQuery.Mikrotik.Command;
using CAMS.CommandQuery.Mikrotik.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenIddict.Validation.AspNetCore;

namespace CAMS.Controllers;

[ApiController]
[Route("[controller]")]
public class MikrotikController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public MikrotikController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    protected async Task<ApiResponse> GetClientIPAddress(string request = null)
    {
        //HttpContext context = HttpContext.Current;
        //string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //if (!string.IsNullOrEmpty(ipAddress))
        //{
        //    string[] addresses = ipAddress.Split(',');
        //    if (addresses.Length != 0)
        //    {
        //        return addresses[0];
        //    }
        //}

        //return context.Request.ServerVariables["REMOTE_ADDR"];

        var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
        //var localIpAddress = httpConnectionFeature?.LocalIpAddress;
        var localIpAddress = httpConnectionFeature?.RemoteIpAddress;
        var ip = localIpAddress.ToString();
        var meidatorObj = await _mediator.Send(new IpWhiteListedCheckQuery() { Ip = ip, RequestStr = request});
        return meidatorObj;
    }

    [HttpPost("GetUserInfo")]
    public async Task<IActionResult> GetUserInfo(GetUserInfoFromMikrotikRequestModel model)
    {
        var meidatorObj = await _mediator.Send(new GetUserInfoFromMikrotikQuery(){model = model});
        return Ok(meidatorObj);
    }

    [HttpGet("GetAllUsersInfo")]
    public IActionResult GetAllUsersInfo([FromQuery] MikrotikRouterFilterParams model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = GetClientIPAddress(req).Result;
        //if(ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = _mediator.Send(new GetAllUsersInfoFromMikrotikQuery() { model = model });
        return Ok(meidatorObj);
    }

    [HttpPost("AddUserInfo")]
    public async Task<IActionResult> AddUserInfo(AddUserInfoInMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = await GetClientIPAddress(req);
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = await _mediator.Send(new AddUserInfoInMikrotikCommand(){model = model});
        return Ok(meidatorObj);
    }

    [HttpGet("AddUserInfoSync")]
    public ActionResult AddUserInfoSync([FromQuery] AddUserInfoInMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = await GetClientIPAddress(req);
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = _mediator.Send(new AddUserInfoMikrotikSyncCommand { model = model });
        return Ok(meidatorObj);
    }

    [HttpPost("DeleteUser")]
    public async Task<IActionResult> DeleteUser(DeleteUserFromMikrotikRouerRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = await GetClientIPAddress(req);
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = await _mediator.Send(new DeleteUserFromMikrotikCommand() { model = model });
        return Ok(meidatorObj);
    }

    [HttpPost("BlockUser")]
    public async Task<IActionResult> BlockUser(BlockUserFromMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = await GetClientIPAddress(req);
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = await _mediator.Send(new BlockUserFromMikrotikCommand(){model = model});
        return Ok(meidatorObj);
    }
    [HttpPost("UnblockUser")]
    public async Task<IActionResult> UnblockUser(UnblockUserFromMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = await GetClientIPAddress(req);
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = await _mediator.Send(new UnblockUserFromMikrotikCommand(){model = model});
        return Ok(meidatorObj);
    }   
    
    [HttpGet("GetUserBlock")]
    public async Task<IActionResult> GetUserBlock([FromQuery] BlockUserFromMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = await GetClientIPAddress(req);
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = await _mediator.Send(new BlockUserFromMikrotikCommand(){model = model});
        return Ok(meidatorObj);
    }
    [HttpGet("GetUserUnblock")]
    public async Task<IActionResult> GetUserUnblock([FromQuery] UnblockUserFromMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = await GetClientIPAddress(req);
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = await _mediator.Send(new UnblockUserFromMikrotikCommand(){model = model});
        return Ok(meidatorObj);
    }

    [HttpGet("SyncUserBlock")]
    public IActionResult SyncUserBlock([FromQuery] BlockUserFromMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = GetClientIPAddress(req).Result;
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = _mediator.Send(new SyncUserBlockCommand() { model = model });
        return Ok(meidatorObj);
    }

    [HttpGet("SyncUserUnblock")]
    public IActionResult SyncUserUnblock([FromQuery] UnblockUserFromMikrotikRouterRequestModel model)
    {
        var req = JsonConvert.SerializeObject(model);
        //var ipCheck = GetClientIPAddress(req).Result;
        //if (ipCheck.StatusCode != 200)
        //{
        //    return Ok(ipCheck);
        //}
        var meidatorObj = _mediator.Send(new SyncUserUnblockCommand() { model = model });
        return Ok(meidatorObj);
    }

    
    [HttpGet("noAuth")]
    public IActionResult noAuth()
    {
        return Ok("this is no authanticate.");
    }

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("auth")]
    public IActionResult auth()
    {
        return Ok("this is authanticate.");
    }

    [HttpGet("SyncListsUserBlock")]
    public IActionResult SyncListsUserBlock([FromQuery] BlockUserListsFromMikrotikRouterRequestModel model)
    {
        var meidatorObj = _mediator.Send(new SyncListsUserBlockCommand() { model = model });
        return Ok(meidatorObj);
    }

    [HttpGet("SyncGetAllQueue")]
    public IActionResult SyncQueueSet([FromQuery] GetAllQueueRequestModel model)
    {
        var meidatorObj = _mediator.Send(new SyncGetAllQueueFromMikrotikQuery() { model = model });
        return Ok(meidatorObj);
    }

    [HttpGet("SyncQueueSet")]
    public IActionResult SyncQueueSet([FromQuery] SetQueueIntoMikrotikRouterRequestModel model)
    {
        var meidatorObj = _mediator.Send(new SetQueueIntoMikrotikRouterCommand() { model = model });
        return Ok(meidatorObj);
    }

    [HttpGet("test")]
    public IActionResult testInterface([FromQuery] MikrotikRouterFilterParams model)
    {
        var meidatorObj = _mediator.Send(new MikrotikRouterInterfaceQuery() { model = model });
        return Ok(meidatorObj);
    }
}