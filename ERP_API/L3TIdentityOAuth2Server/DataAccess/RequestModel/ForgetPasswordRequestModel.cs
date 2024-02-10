using FluentValidation;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

public class ForgetPasswordRequestModel
{
    public string Email { get; set; }
}

public class ForgetPasswordRequestModelValidator : AbstractValidator<ForgetPasswordRequestModel>
{
    public ForgetPasswordRequestModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
    }
}