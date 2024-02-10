using AutoMapper;
using IPService.CommandQuery.Command;
using IPService.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IPService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GateWayIpAddressController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public GateWayIpAddressController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }



        [HttpPost("AddIp")]
        public async Task<IActionResult> AddGateWayIp(AddGateWayIpReq ReqModel)
        {
            var response =  await _mediator.Send(new AddGateWayIpCommand(){model = ReqModel });                               
            return Ok(response);
        }

        [HttpGet("GetAllIp")]
        public async Task<IActionResult> GetAllIpInfo()
        {
            
            
           var GatewayIpAddresses = await _mediator.Send(new GetAllGateWayIpQuery());
            return Ok(GatewayIpAddresses);
        }

        [HttpGet("GetIpById")]
        public async Task<IActionResult> GetIpInfoById(long id)
        {

            var GatewayIpAddress = await _mediator.Send(new GetGatewayIpByIdQuery { Id = id });
            return Ok(GatewayIpAddress);
        }


        [HttpPost("UpdateIp")]
        public async Task<IActionResult> UpdateGateWayIp(UpdateGatewayIpReq ReqModel)
        {
            var response = await _mediator.Send(new UpdateGatewayIpCommand() {model = ReqModel });
            return Ok(response);
        }


        [HttpPost("ChangeIpStatus")]
        public async Task<IActionResult> IpStatusChange(long id,bool status)
        {

            var ChangeStatusIpAddress = await _mediator.Send(new ChangeGatewayIpCommand { Id = id,Status = status });
            return Ok(ChangeStatusIpAddress);
        }

    }
}
