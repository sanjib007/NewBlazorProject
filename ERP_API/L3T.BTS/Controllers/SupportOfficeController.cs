using AutoMapper;
using L3T.BTS.CommandQuery.BTS.Command;
using L3T.BTS.CommandQuery.BTS.Queries;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.BTS;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace L3T.BTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportOfficeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public SupportOfficeController(IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("AddSupportOffice")]
        public async Task<IActionResult> AddSupportOffice(SupportOfficeDTO supportOfficeObj)
        {
            try
            {
                //SupportOffice supportOffice = new SupportOffice();
                //supportOfficeObj.InsertedBy = "Admin";
                //supportOfficeObj.InsertedDateTime = DateTime.Now;

                //ApiResponse apiResponse = await _mediator.Send(new AddSupportOfficeCommand { model = supportOfficeObj });
                ApiResponse apiResponse = await _mediator.Send(new AddSupportOfficeCommand { model = _mapper.Map<SupportOffice>(supportOfficeObj) });


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
        public async Task<IActionResult> Update(int id, SupportOfficeDTO command)
        {

            try
            {
                var supportOfficeObj = await _mediator.Send(new UpdateSupportOfficeCommand { Id = id, supportOffice = _mapper.Map<SupportOffice>(command) });

                return Ok(supportOfficeObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetSupportOfficeById")]
        [AcceptVerbs("POST")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSupportOfficeById(int id)
        {
            try
            {
                var supportOffice = await _mediator.Send(new GetSupportOfficeByIdQuery { Id = id });
                if (supportOffice == null)
                {
                    return Ok("Support Office not found");
                }
                else
                {
                    return Ok(supportOffice);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AcceptVerbs("POST")]
        [Route("GetAllSupportOffice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSupportOffice()
        {
            try
            {
                var supportOffice = await _mediator.Send(new GetAllSupportOfficeQuery());
                if (supportOffice == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(supportOffice);
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
                var medobj = await _mediator.Send(new DeleteSupportOffice { Id = id });
                return Ok(medobj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
