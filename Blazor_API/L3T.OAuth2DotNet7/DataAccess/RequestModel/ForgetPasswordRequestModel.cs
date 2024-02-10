using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

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