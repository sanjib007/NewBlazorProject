using L3T.Infrastructure.Helpers.Implementation.Ticket;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class TicketServiceDependency
    {
        public static IServiceCollection AddTicketServiceDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(ITicketService), typeof(TicketService));
            return services;
        }
    }
}
