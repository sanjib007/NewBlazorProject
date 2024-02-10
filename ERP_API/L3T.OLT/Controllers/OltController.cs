using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.BTS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using L3T.Infrastructure.Helpers.Models.OLT;
using L3T.Infrastructure.Helpers.Models.OLT.OltDTO;
using L3T.OLT.CommandQuery.OLT.Command;

namespace L3T.OLT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OltController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public OltController(IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("AddOlt")]
        public async Task<IActionResult> AddOlt(OltInfoDTO model)
        {
            try
            {
                OltInfo olt = new OltInfo();
                olt.OltName = model.OltName;
                olt.OltCode = model.OltCode;
                olt.InsertedBy = "Admin";
                olt.InsertedDateTime = DateTime.Now;

                ApiResponse apiResponse = await _mediator.Send(new AddOLTCommand { model = olt });


                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
