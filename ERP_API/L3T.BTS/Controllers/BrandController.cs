using AutoMapper;
using L3T.BTS.CommandQuery.BTS.Command;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.MessageBroker.Models.BTS;
using L3T.Infrastructure.Helpers.Models.BTS.BtsDTO;
using L3T.Infrastructure.Helpers.Models.BTS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using Newtonsoft.Json;
using L3T.BTS.CommandQuery.BTS.Queries;
using L3T.Infrastructure.Helpers.Models.TicketEntity.TicketDTO;

namespace L3T.BTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BrandController(IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("AddBrand")]
        public async Task<IActionResult> AddBrand(BrandDTO model)
        {

            try
            {
                //Brand brand = new Brand();
                //brand.Name = model.Name;
                //brand.ProductType = model.ProductType;
                //brand.InsertedBy = "Admin";
                //brand.InsertedDateTime = DateTime.Now;
                //await Mediator.Send(new CreateStudentCommand { Student = _mapper.Map<Student>(studentmodel) })
                ApiResponse apiResponse = await _mediator.Send(new AddBrandCommand { model = _mapper.Map<Brand>(model) });


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
        public async Task<IActionResult> Update(int id, BrandDTO command)
        {

            try
            {
                //var brand = _mapper.Map<Brand>(command);

                var brandobj = await _mediator.Send(new UpdateBrandCommand { Id = id, brand = _mapper.Map<Brand>(command) });

                return Ok(brandobj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetBrandById")]
        [AcceptVerbs("POST")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                var brandobj = await _mediator.Send(new GetBrandByIdQuery { Id = id });
                if (brandobj == null)
                {
                    return Ok("Brand not found");
                }
                else
                {
                    return Ok(brandobj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AcceptVerbs("POST")]
        [Route("GetAllBrand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBrand()
        {
            try
            {
                var brandobj = await _mediator.Send(new GetAllBrandQuery());
                if (brandobj == null)
                {
                    return Ok("No data found");
                }
                else
                {
                    return Ok(brandobj);
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
                var medobj = await _mediator.Send(new DeleteBrandByIdCommand { Id = id });
                return Ok(medobj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
