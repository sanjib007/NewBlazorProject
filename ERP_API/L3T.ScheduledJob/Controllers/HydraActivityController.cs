using L3T.Infrastructure.Helpers.Interface.Schedule;
using L3T.Infrastructure.Helpers.Models.Schedule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L3T.ScheduledJob.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HydraActivityController : ControllerBase
	{
		private readonly ILogger<HydraActivityController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IScheduleService _scheduleService;
		public HydraActivityController(
			ILogger<HydraActivityController> logger,
			IConfiguration configuration,
			IScheduleService scheduleService)
		{
			_logger = logger;
			_configuration = configuration;
			_scheduleService = scheduleService;
		}

		[HttpGet]
		[Route("CheckandUpdateCustomer")]
		public async Task<IActionResult> CheckandUpdateCustomer()
		{
			_logger.LogInformation("--------------------------------------------------------");
			_logger.LogInformation("CheckandUpdateCustomer");
			try
			{

				DateTime expireDate = Convert.ToDateTime("1/1/1900");//DateTime.Now.AddMonths(18);
				List<NetworkInformation> hydraLists = await _scheduleService.GetInactiveHydraCustomer(expireDate);

				foreach (NetworkInformation dr in hydraLists)
				{
					string customerid = dr.CustomerID.ToString();
					if (!string.IsNullOrEmpty(customerid))
					{
						bool paymentTrxID = await _scheduleService.UpdateInactiveHydraCustomer(customerid);
						if (paymentTrxID)
						{
							var status = await _scheduleService.UpdateHydra_CustomerStatus(customerid, 1);
						}
						else
						{
							var status = await _scheduleService.UpdateHydra_CustomerStatus(customerid, 3);
						}
					}

				}
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogInformation("CheckandUpdateCustomer " + " Exception:  " + ex.ToString());
				return BadRequest();
			}
		}


	}
}
