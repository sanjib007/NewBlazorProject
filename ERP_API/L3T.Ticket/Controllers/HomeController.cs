using AutoMapper;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.MessageBroker.Models.Ticket;

using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using L3T.Ticket.CommandQuery.Ticket.CommandObject.Commands;
using L3T.Ticket.CommandQuery.Ticket.CommandObject.Queries;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using L3T.Infrastructure.Helpers.Models.TicketEntity.TicketDTO;

namespace L3T.Ticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITicketService _ticketService;
        private readonly IConfiguration _configuration;
        private IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _endpoint;

        public HomeController(ILogger<HomeController> logger,
                              ITicketService ticketService,
                              IMapper mapper,
                              IPublishEndpoint endpoint,
                              IMediator mediator)
        {
            _logger = logger;
            _ticketService = ticketService;
            _mapper = mapper;
            _endpoint = endpoint;
            _mediator = mediator;
        }


        //[AcceptVerbs("POST", "GET")]
        //[Route("AddTicketEntry")]
        //public async Task<IActionResult> AddTicketEntry(TicketEntry model)
        //{
        //    if (model == null || !ModelState.IsValid)
        //        return BadRequest();

        //    object returnMessageObject = string.Empty;
        //    var group = "1";
        //    _logger.LogInformation("New request  submitted. Request is: {@model}", model);
        //    try
        //    {
        //        var checkParamObj = checkParameter(model);
        //        model.ForwardDate = DateTime.Now;

        //        if (checkParamObj.Status == 200)
        //        {

        //            await _ticketService.TicketEntrySubmitAsync(model);
        //            return (Ok());
        //        }
        //        else
        //        {
        //            returnMessageObject = new
        //            {
        //                Message = checkParamObj.Message,
        //                Status = checkParamObj.Status
        //            };
        //            _logger.LogInformation("Status -> Error: " + returnMessageObject.ToString());
        //            return StatusCode((int)HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(returnMessageObject));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errormessage = "Exception occurred: " + ex.Message.ToString();
        //        returnMessageObject = new
        //        {
        //            Message = "Communication with server failed. Please try after some time",
        //            status = 501
        //        };

        //        _logger.LogInformation("Status ->" + returnMessageObject.ToString());
        //        //_mailrepo.sendMail(ex.ToString(), "Error occure on AddNewAccount Method: " + model.PhoneNumber.Trim());
        //        return StatusCode((int)HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(returnMessageObject));
        //    }
        //}

        [AcceptVerbs("POST")]
        [Route("AddTicketEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(TicketEntryDTO TicketEntry)
        {
            try
            {
                var meidatorObj = await _mediator.Send(new CreateTicketCommand { TicketEntry = _mapper.Map<TicketEntry>(TicketEntry) });

                _endpoint.Publish<MB_TicketEntry>(TicketEntry);

                return Ok(meidatorObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AcceptVerbs("POST")]
        [Route("GetAllTicket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            try
            {                
                var medobj = await _mediator.Send(new GetAllTicketQuery());
                if (medobj == null)
                {
                    return Ok("No data found");
                }
                else {
                    return Ok(medobj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetTicketById")]
        [AcceptVerbs("POST")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var medobj = await _mediator.Send(new GetTicketByIdQuery { Id = id });
                if (medobj == null)
                {
                    return Ok("Ticket not found");
                }
                else
                {
                    return Ok(medobj);
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
                var medobj = await _mediator.Send(new DeleteTicketByIdCommand { Id = id });
                return Ok(medobj);
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
        public async Task<IActionResult> Update(int id, TicketUpdateDTO command)
        {

            try
            {
                var TicketEntry = _mapper.Map<TicketEntry>(command);

                var medobj = await _mediator.Send(new UpdateTicketCommand { Id = id, TicketEntry = TicketEntry });

                return Ok(medobj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Private Method
        private dynamic checkParameter(TicketEntry ticketEntry)
        {

            _logger.LogInformation("checkParam start ");
            var Message = "";
            var Status = 500;
            dynamic returnMessageObject = string.Empty;

            if (ticketEntry.brCliCode == "" || ticketEntry.MqID == "")
            {
                Message = "brCliCode or MqID is required";
            }

            if (string.IsNullOrEmpty(ticketEntry.ReceiveDateTime.ToString()))
            {
                Message = "ReceiveDateTime is required";
            }

            else if (string.IsNullOrEmpty(ticketEntry.FaultOccured.ToString()))
            {
                Message = "FaultOccured is required";
            }
            else if (ticketEntry.SourceOfInformation == "<--Select-->")
            {
                Message = "SourceOfInformation is required";
            }
            else if (ticketEntry.SourceOfInformation == "Client (by Phone)")
            {
                if (ticketEntry.SourceMobileNo == "")
                {
                    Message = "Customer Calling Number is required";

                }
                else if (ticketEntry.SourceMobileNo.Length != 11)
                {
                    Message = "Invalid Mobile Number";
                }

            }
            else if (string.IsNullOrEmpty(ticketEntry.Complains))
            {
                Message = "Complains is required";
            }
            else if (ticketEntry.TaskCategory == -1)
            {
                Message = "ComplainCategory is required";
            }
            else if (ticketEntry.TaskCategory != 21)
            {
                if (ticketEntry.PendingTeamID == "<--Select-->")
                {
                    Message = "Select Missing Forward Team";
                }
            }
            else if (ticketEntry.TaskNature == -1)
            {
                Message = "Select Missing Task Nature";
            }
            else
            {
                Status = 200;
                Message = "Parameter is correct";
            }

            returnMessageObject = new
            {
                Message = Message,
                Status = Status
            };
            _logger.LogInformation("checkParam End message: " + Message);
            _logger.LogInformation("checkParam End status: " + Status);

            return returnMessageObject;
        }
        private bool validatetoken(string concatparam, string tokenKey)
        {
            try
            {
                tokenKey = tokenKey.ToLower();
                var ownKey = CreateMD5(concatparam);
                if (ownKey == tokenKey)
                {
                    return true;
                }
                else
                {
                    _logger.LogInformation("ownKey: " + concatparam + " tokenKey: " + tokenKey);
                    return false;
                }
            }
            catch (Exception ex)
            {
                string errormessage = "Error : " + ex.Message.ToString();
                _logger.LogInformation("ZangiServiceController/validatetoken " + errormessage);
                return false;
            }
        }
        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var provider = System.Security.Cryptography.MD5.Create())
                {
                    StringBuilder builder = new StringBuilder();

                    foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(input)))
                        builder.Append(b.ToString("x2").ToLower());

                    return builder.ToString();
                }
            }
        }
        #endregion
        
        
    }
}
