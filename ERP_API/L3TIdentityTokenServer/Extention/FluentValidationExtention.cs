using FluentValidation;
using FluentValidation.AspNetCore;
using L3TIdentityTokenServer.DataAccess.RequestModel;

namespace L3TIdentityTokenServer.Extention;

public static class FluentValidationExtention
{
    public static IServiceCollection AddFluentValidationCollection(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<RegisterRequestModel>, RegisterViewModelValidator>();
        services.AddScoped<IValidator<ChangePasswordRequestModel>, ChangePasswordRequestModelValidator>();
        services.AddScoped<IValidator<ForgetPasswordRequestModel>, ForgetPasswordRequestModelValidator>();
        services.AddScoped<IValidator<ChangePhoneNumberRequestModel>, ChangePhoneNumberRequestModelValidator>();
        services.AddScoped<IValidator<ConfirmPhoneNumberRequestModel>, ConfirmPhoneNumberRequestModelValidator>();
        services.AddScoped<IValidator<ChangeEmailRequestModel>, ChangeEmailRequestModelValidator>();
        services.AddScoped<IValidator<GetUserClaimsRequestModel>, GetUserClaimsRequestModelValidator>();
        services.AddScoped<IValidator<GetUserRolesRequestModel>, GetUserRolesRequestModelValidator>();
        services.AddScoped<IValidator<EditRoleNameRequestModel>, EditRoleNameRequestModelValidator>();
        return services;
    }
}