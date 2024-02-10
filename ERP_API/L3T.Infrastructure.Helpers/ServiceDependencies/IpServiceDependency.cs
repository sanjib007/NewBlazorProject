using L3T.Infrastructure.Helpers.Implementation.BTS;
using L3T.Infrastructure.Helpers.Implementation.Cams;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class IpServiceDependency
    {
        public static IServiceCollection AddIpServiceDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGateWayIpService), typeof(GateWayIpService));
            services.AddScoped(typeof(IGatewayWiseClientIpService), typeof(GatewayWiseClientIpService));

            return services;
        }
    }
}
