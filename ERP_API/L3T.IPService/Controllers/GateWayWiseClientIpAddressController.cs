using AutoMapper;
using IPService.CommandQuery.Command;
using IPService.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GateWayWiseClientIpAddressController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public GateWayWiseClientIpAddressController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("AddClientIp")]
        public async Task<IActionResult> AddGatewayWiseClientIp(AddGatewayWiseClientIpReq ReqModel)
        {
            var response = await _mediator.Send(new AddGatewayWiseClientIpCommand() { model = ReqModel });
            return Ok(response);
        }


        [HttpGet("GetAllClientIp")]
        public async Task<IActionResult> GetAllGatewayWiseClientIpInfo()
        {


            var GatewayIpAddresses = await _mediator.Send(new GetAllGatewayWiseClientIpQuery());
            return Ok(GatewayIpAddresses);
        }


        [HttpGet("GetClientIpById")]
        public async Task<IActionResult> GetClientIpInfoById(long id)
        {

            var GatewayIpAddress = await _mediator.Send(new GetGatewayWiseClientIpByIdQuery { Id = id });
            return Ok(GatewayIpAddress);
        }

        [HttpPost("UpdateClientIp")]
        public async Task<IActionResult> UpdateGatewayWiseClientIp(UpdateGatewayWiseClientIpReq ReqModel)
        {
            var response = await _mediator.Send(new UpdateGatewayWiseClientIpCommand() { model = ReqModel });
            return Ok(response);
        }


        [HttpPost("ChangeClientIpStatus")]
        public async Task<IActionResult> ClientIpStatusChange(long id, bool status, string lastModifiedBy)
        {

            var ChangeStatusIpAddress = await _mediator.Send(new ChangeGatewayWiseIpCommand { Id = id, Status = status, LastModifiedBy = lastModifiedBy });
            return Ok(ChangeStatusIpAddress);
        }
    }
}
