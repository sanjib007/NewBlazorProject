using FluentValidation;

namespace L3TIdentityTokenServer.DataAccess.RequestModel;

public class ConfirmEmailRequestModel
{
    public string Token { get; set; }
    public string Email { get; set; }
}
public class ConfirmEmailRequestModelValidator : AbstractValidator<ConfirmEmailRequestModel>
{
    public ConfirmEmailRequestModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}