using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L3T.Ticket.Consumers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [AcceptVerbs("POST", "GET")]
        [Route("test")]
        public async Task<IActionResult> test()
        {
            return Ok();
        }
     }
}
