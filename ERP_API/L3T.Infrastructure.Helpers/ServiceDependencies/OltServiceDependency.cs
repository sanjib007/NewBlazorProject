using L3T.Infrastructure.Helpers.Implementation.OLT;
using L3T.Infrastructure.Helpers.Interface.OLT;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class OltServiceDependency
    {
        public static IServiceCollection AddOltServiceDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(IOltService), typeof(OltService));
            return services;
        }
    }
}
