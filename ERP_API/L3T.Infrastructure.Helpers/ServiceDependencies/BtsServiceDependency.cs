using L3T.Infrastructure.Helpers.Implementation.BTS;
using L3T.Infrastructure.Helpers.Interface.BTS;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class BtsServiceDependency
    {
        public static IServiceCollection AddBtsServiceDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBtsService), typeof(BtsService));
            return services;
        }
    }
}
