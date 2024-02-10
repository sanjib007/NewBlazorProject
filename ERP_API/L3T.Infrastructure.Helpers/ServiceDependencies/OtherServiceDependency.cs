using L3T.Infrastructure.Helpers.Implementation.Email;
using L3T.Infrastructure.Helpers.Implementation.FieldForce;
using L3T.Infrastructure.Helpers.Implementation.ThirdParty;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Interface.ThirdParty;
using Microsoft.Extensions.DependencyInjection;

namespace L3T.Infrastructure.Helpers.ServiceDependencies
{
    public static class OtherServiceDependency
    {
        public static IServiceCollection AddOtherServiceDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(IOtherService), typeof(OtherService));

            return services;
        }
    }
}
