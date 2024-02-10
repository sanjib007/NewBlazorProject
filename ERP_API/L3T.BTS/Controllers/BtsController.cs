using AutoMapper;
using L3T.BTS.CommandQuery.BTS.Command;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.MessageBroker.Models.BTS;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace L3T.BTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BtsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _endpoint;
        public BtsController(IMediator mediator, 
            IMapper mapper,
            IPublishEndpoint endpoint)
        {
            _mediator = mediator;
            _mapper = mapper;
            _endpoint= endpoint;
        }

        [HttpPost("AddBtsInfo")]
        public async Task<IActionResult> AddBtsInfo(BtsInfoDTO model)
        {
            
            try
            {
                ApiResponse apiResponse = await _mediator.Send(new AddBtsInfoCommand { model = _mapper.Map<BtsInfo>(model) });

               return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateBtsInfo")]
        public async Task<IActionResult> UpdateBtsInfo(int id, BtsUpdateDTO command)
        {
            try
            {
                var btsinfom = _mapper.Map<BtsInfo>(command);

                var medobj = await _mediator.Send(new UpdateBtsCommand { Id = id, BtsObj = btsinfom });

                return Ok(medobj);
                //ApiResponse apiResponse = await _mediator.Send(new UpdateBtsCommand { model = _mapper.Map<BtsInfo>(model) });

                //_endpoint.Publish<MB_BtsInfo>(model);

                //return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //var meidatorObj = await _mediator.Send(new AddUserInfoInMikrotikCommand() { model = model });
            //return Ok(meidatorObj);
        }
    }
}
