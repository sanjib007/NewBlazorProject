using L3T.Infrastructure.Helpers.Implementation.FieldForce;
using L3T.Infrastructure.Helpers.Implementation.SelfCare;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Interface.SelfCare;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class SelfCareServiceDependency
    {
        public static IServiceCollection AddSelfCareServiceDependecy(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISelfCareService), typeof(SelfCareService));

            return services;
        }

    }
}
