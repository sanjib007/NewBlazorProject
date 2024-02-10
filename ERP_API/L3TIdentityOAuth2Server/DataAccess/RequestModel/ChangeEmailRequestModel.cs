using FluentValidation;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

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