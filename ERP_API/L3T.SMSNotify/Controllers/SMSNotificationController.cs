using L3T.SMSNotify.CommandQuery.SmsNotify.Command;
using L3T.SMSNotify.CommandQuery.SmsNotify.Queues;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace L3T.SMSNotify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SMSNotificationController : Controller
{
    private readonly IMediator _mediator;
    public SMSNotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetSmsNotification")]
    public async Task<IActionResult> Index()
    {
        var meidatorObj = await _mediator.Send(new GetSMSNotificationQuery());
        return Ok(meidatorObj);
    }

    [HttpGet("SmsPushInMySqlDB")]
    public async Task<IActionResult> SmsPushInMySqlDB()
    {
        var meidatorObj = await _mediator.Send(new SmsPushInMySqlDBCommand());
        return Ok(meidatorObj);
    }

    //[HttpGet("test")]
    //public async Task<IActionResult> test()
    //{
    //    var data = "8801969904293";
    //    var testdata = data.Substring(2, 11);
    //    return Ok(testdata);
    //}

}