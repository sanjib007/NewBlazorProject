using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.CommonModel
{
   
        //[ApiVersion("2.0")]
        //[Route("api/v{v:apiVersion}/[controller]")]

        [Route("api/[controller]")]
        [ApiController]
        public class ApplicationBaseController : ControllerBase
        {

        }
    
}
