using FluentValidation;
using FluentValidation.AspNetCore;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;

namespace L3TIdentityOAuth2Server.Extention;

public static class FluentValidationExtention
{
    public static IServiceCollection AddFluentValidationCollection(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddTransient<IValidator<RegisterRequestModel>, RegisterViewModelValidator>();
        services.AddTransient<IValidator<ChangePasswordRequestModel>, ChangePasswordRequestModelValidator>();
        services.AddTransient<IValidator<ForgetPasswordRequestModel>, ForgetPasswordRequestModelValidator>();
        services.AddTransient<IValidator<ChangePhoneNumberRequestModel>, ChangePhoneNumberRequestModelValidator>();
        services.AddTransient<IValidator<ConfirmPhoneNumberRequestModel>, ConfirmPhoneNumberRequestModelValidator>();
        services.AddTransient<IValidator<ChangeEmailRequestModel>, ChangeEmailRequestModelValidator>();
        services.AddTransient<IValidator<GetUserClaimsRequestModel>, GetUserClaimsRequestModelValidator>();
        services.AddTransient<IValidator<GetUserRolesRequestModel>, GetUserRolesRequestModelValidator>();
        services.AddTransient<IValidator<EditRoleNameRequestModel>, EditRoleNameRequestModelValidator>();
        services.AddTransient<IValidator<UserRequestModel>, UserRequestModelValidator>();
        services.AddTransient<IValidator<PushNotificationRequestModel>, PushNotificationRequestModelValidator>();
        return services;
    }
}