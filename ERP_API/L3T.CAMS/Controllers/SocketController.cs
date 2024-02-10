using AutoMapper;
using CAMS.CommandQuery.Mikrotik.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace CAMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SocketController(IMediator mediator, IMapper mapper)
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
            var meidatorObj = await _mediator.Send(new IpWhiteListedCheckQuery() { Ip = ip, RequestStr = request });
            return meidatorObj;
        }


        [HttpPost("CallSocket")]
        public async Task<IActionResult> GetLiveDataFromRouter(GetUserInfoFromMikrotikRequestModel model)
        {
            var meidatorObj = await _mediator.Send(new GetLiveDataForACustomerQuery() { model = model });
            return Ok(meidatorObj);
        }

    }
}
