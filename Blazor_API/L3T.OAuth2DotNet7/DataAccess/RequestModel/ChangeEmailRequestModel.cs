using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class ChangeEmailRequestModel
{
    public string Email { get; set; }
}

public class ChangeEmailRequestModelValidator : AbstractValidator<ChangeEmailRequestModel>
{
    public ChangeEmailRequestModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull();
    }
}