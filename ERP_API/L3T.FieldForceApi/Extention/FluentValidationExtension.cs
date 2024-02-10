using FluentValidation;
using FluentValidation.AspNetCore;
using L3T.FieldForceApi.FluentValidator;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;

namespace L3T.FieldForceApi.Extention
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection AddFluentValidationCollectionExtenstion(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            // services.AddTransient<IValidator<RegisterRequestModel>, RegisterViewModelValidator>();
            services.AddTransient<IValidator<SendSmsRequestModel>, SendSmsRequestModelValidator>();

            return services;
        }
    }
}
