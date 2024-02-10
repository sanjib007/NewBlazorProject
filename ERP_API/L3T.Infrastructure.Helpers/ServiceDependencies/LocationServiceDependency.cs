using L3T.Infrastructure.Helpers.Implementation.Location;
using L3T.Infrastructure.Helpers.Interface.Location;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class LocationServiceDependency
    {
        public static IServiceCollection AddLocationServiceDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILocationService), typeof(LocationService));
            return services;
        }
    }
}
