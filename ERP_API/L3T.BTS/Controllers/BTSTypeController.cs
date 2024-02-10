using AutoMapper;
using L3T.BTS.CommandQuery.BTS.Command;
using L3T.BTS.CommandQuery.BTS.Queries;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.BTS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace L3T.BTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BTSTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BTSTypeController(IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("AddBTSType")]
        public async Task<IActionResult> AddBTSType(BTSTypeDTO btsTypeObj)
        {
            try
            {
                //SupportOffice supportOffice = new SupportOffice();
                //supportOfficeObj.InsertedBy = "Admin";
                //supportOfficeObj.InsertedDateTime = DateTime.Now;

                //ApiResponse apiResponse = await _mediator.Send(new AddSupportOfficeCommand { model = supportOfficeObj });
                ApiResponse apiResponse = await _mediator.Send(new AddBTSTypeCommand { model = _mapper.Map<BTSType>(btsTypeObj) });


                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AcceptVerbs("POST")]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, BTSTypeDTO command)
        {

            try
            {
                var btsTypeObj = await _mediator.Send(new UpdateBTSTypeCommand { Id = id, btsType = _mapper.Map<BTSType>(command) });

                return Ok(btsTypeObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetBTSTypeById")]
        [AcceptVerbs("POST")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBTSTypeById(int id)
        {
            try
            {
                var btsType = await _mediator.Send(new GetBTSTypeByIdQuery { Id = id });
                if (btsType == null)
                {
                    return Ok("Support Office not found");
                }
                else
                {
                    return Ok(btsType);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AcceptVerbs("POST")]
        [Route("GetAllBTSType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBTSType()
        {
            try
            {
                var btsType = await _mediator.Send(new GetAllBTSTypeQuery());
                if (btsType == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(btsType);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AcceptVerbs("POST")]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var medobj = await _mediator.Send(new DeleteBTSType { Id = id });
                return Ok(medobj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
