using L3T.Infrastructure.Helpers.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.Schedule
{
	public interface IScheduleService
	{
		Task<List<NetworkInformation>> GetInactiveHydraCustomer(DateTime exDate);
		Task<bool> UpdateInactiveHydraCustomer(string customerid);
		Task<NetworkInformation> UpdateHydra_CustomerStatus(string customerid, int status);
	}
}
