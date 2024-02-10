using L3T.Infrastructure.Helpers.Implementation.Schedule;
using L3T.Infrastructure.Helpers.Interface.Schedule;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
	public static class ScheduledServiceDependency
	{
		public static IServiceCollection AddScheduledServiceDependency(this IServiceCollection services)
		{
			services.AddScoped(typeof(IScheduleService),typeof(ScheduleService));
			return services;
		}
	}
}
