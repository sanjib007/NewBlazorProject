using FluentValidation;
using FluentValidation.AspNetCore;
using L3T.Infrastructure.Helpers.Models.RequestModel;

namespace L3T.Client.API.Extention;

public static class FluentValidationExtention
{
    public static IServiceCollection AddFluentValidationCollection(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(); 
        
        return services;
    }
}