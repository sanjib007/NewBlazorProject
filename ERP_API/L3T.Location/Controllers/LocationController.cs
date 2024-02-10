using AutoMapper;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Location.CommandQuery.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L3T.Infrastructure.Helpers.Models.Location;
using L3T.Location.CommandQuery.Command;
using System.Threading.Tasks;
using System;

namespace L3T.Location.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [AcceptVerbs("POST")]
        [Route("GetAllZone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllZone()
        {
            try
            {
                var ZoneObj = await _mediator.Send(new GetAllZoneQuery());
                if (ZoneObj == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(ZoneObj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddZone")]
        public async Task<IActionResult> AddZone(Zone model)
        {

            try
            {
                //model.ins   .InsertedBy = "Admin";
                //zone.InsertedDateTime = DateTime.Now;

                ApiResponse apiResponse = await _mediator.Send(new AddZoneCommand { model = model });


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
        public async Task<IActionResult> Update(int id, Zone command)
        {

            try
            {
                //var brand = _mapper.Map<Brand>(command);

                var Zoneobj = await _mediator.Send(new UpdateZoneCommand { Id = id, model = command });

                return Ok(Zoneobj);
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
                var medobj = await _mediator.Send(new DeleteZoneByIdCommand { Id = id });
                return Ok(medobj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AcceptVerbs("POST")]
        [Route("GetAllDivision")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDivision()
        {
            try
            {
                var divisionobj = await _mediator.Send(new GetAllDivisionQuery());
                if (divisionobj == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(divisionobj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AcceptVerbs("POST")]
        [Route("GetAllDistrict")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDistrict()
        {
            try
            {
                var districtobj = await _mediator.Send(new GetAllDistrictQuery());
                if (districtobj == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(districtobj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AcceptVerbs("POST")]
        [Route("GetAllUpazila")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUpazila()
        {
            try
            {
                var UpazilaObj = await _mediator.Send(new GetAllUpazilaQuery());
                if (UpazilaObj == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(UpazilaObj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AcceptVerbs("POST")]
        [Route("GetUpazilaByDistrictId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUpazilaByDistrictId(long districtId)
        {
            try
            {
                var UpazilaObj = await _mediator.Send(new GetUpazilaByDistrictIdQuery{ districtId = districtId });
                if (UpazilaObj == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(UpazilaObj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [AcceptVerbs("POST")]
        [Route("GetDistrictByDivisionId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDistrictByDivisionId(long divisionId)
        {
            try
            {
                var DistrictObj = await _mediator.Send(new GetDistrictByDivisionIdQuery { divisionId = divisionId });
                if (DistrictObj == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(DistrictObj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
